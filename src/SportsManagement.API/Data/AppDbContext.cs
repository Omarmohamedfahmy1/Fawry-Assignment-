using Microsoft.EntityFrameworkCore;
using SportsManagement.API.Models;

namespace SportsManagement.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamStaff> TeamStaff => Set<TeamStaff>();
    public DbSet<PlayerProfile> PlayerProfiles => Set<PlayerProfile>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<CoachingPlan> CoachingPlans => Set<CoachingPlan>();
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<TeamGameStats> TeamGameStats => Set<TeamGameStats>();
    public DbSet<PlayerGameStats> PlayerGameStats => Set<PlayerGameStats>();
    public DbSet<TeamCumulativeStats> TeamCumulativeStats => Set<TeamCumulativeStats>();
    public DbSet<PlayerCumulativeStats> PlayerCumulativeStats => Set<PlayerCumulativeStats>();
    public DbSet<AiLineupSuggestion> AiLineupSuggestions => Set<AiLineupSuggestion>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<FitnessRecord> FitnessRecords => Set<FitnessRecord>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Announcement> Announcements => Set<Announcement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.UserId);
            e.HasIndex(u => u.Username).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Status).HasConversion<string>();

            e.HasMany(u => u.ManagedTeams)
             .WithOne(t => t.Manager)
             .HasForeignKey(t => t.ManagerUserId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(u => u.SentMessages)
             .WithOne(m => m.Sender)
             .HasForeignKey(m => m.SenderUserId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(u => u.ReceivedMessages)
             .WithOne(m => m.Receiver)
             .HasForeignKey(m => m.ReceiverUserId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(u => u.ApprovedBy)
             .WithMany()
             .HasForeignKey(u => u.ApprovedByUserId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ── Role ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<Role>(e =>
        {
            e.HasKey(r => r.RoleId);
            e.HasIndex(r => r.Name).IsUnique();
        });

        // ── UserRole ──────────────────────────────────────────────────────────
        modelBuilder.Entity<UserRole>(e =>
        {
            e.HasKey(ur => ur.UserRoleId);

            e.HasOne(ur => ur.User)
             .WithMany(u => u.UserRoles)
             .HasForeignKey(ur => ur.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ur => ur.Role)
             .WithMany(r => r.UserRoles)
             .HasForeignKey(ur => ur.RoleId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ur => ur.Team)
             .WithMany(t => t.UserRoles)
             .HasForeignKey(ur => ur.TeamId)
             .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(ur => ur.AssignedBy)
             .WithMany()
             .HasForeignKey(ur => ur.AssignedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Team ──────────────────────────────────────────────────────────────
        modelBuilder.Entity<Team>(e =>
        {
            e.HasKey(t => t.TeamId);
        });

        // ── TeamStaff ─────────────────────────────────────────────────────────
        modelBuilder.Entity<TeamStaff>(e =>
        {
            e.HasKey(ts => ts.StaffId);

            e.HasOne(ts => ts.User)
             .WithMany(u => u.StaffRoles)
             .HasForeignKey(ts => ts.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ts => ts.Team)
             .WithMany(t => t.Staff)
             .HasForeignKey(ts => ts.TeamId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── PlayerProfile ─────────────────────────────────────────────────────
        modelBuilder.Entity<PlayerProfile>(e =>
        {
            e.HasKey(pp => pp.PlayerId);

            e.HasOne(pp => pp.User)
             .WithOne(u => u.PlayerProfile)
             .HasForeignKey<PlayerProfile>(pp => pp.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pp => pp.Team)
             .WithMany(t => t.Players)
             .HasForeignKey(pp => pp.TeamId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── Event ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Event>(e =>
        {
            e.HasKey(ev => ev.EventId);

            e.HasOne(ev => ev.Team)
             .WithMany(t => t.Events)
             .HasForeignKey(ev => ev.TeamId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── CoachingPlan ──────────────────────────────────────────────────────
        modelBuilder.Entity<CoachingPlan>(e =>
        {
            e.HasKey(cp => cp.PlanId);

            e.HasOne(cp => cp.CreatorStaff)
             .WithMany(ts => ts.CoachingPlans)
             .HasForeignKey(cp => cp.CreatorStaffId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── TrainingSession ───────────────────────────────────────────────────
        modelBuilder.Entity<TrainingSession>(e =>
        {
            e.HasKey(ts => ts.SessionId);

            e.HasOne(ts => ts.Event)
             .WithOne(ev => ev.TrainingSession)
             .HasForeignKey<TrainingSession>(ts => ts.EventId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ts => ts.Plan)
             .WithMany(cp => cp.TrainingSessions)
             .HasForeignKey(ts => ts.PlanId)
             .OnDelete(DeleteBehavior.SetNull);

            e.HasOne(ts => ts.ConductedByStaff)
             .WithMany(s => s.TrainingSessions)
             .HasForeignKey(ts => ts.ConductedByStaffId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Attendance ────────────────────────────────────────────────────────
        modelBuilder.Entity<Attendance>(e =>
        {
            e.HasKey(a => a.AttendanceId);

            e.HasOne(a => a.Event)
             .WithMany(ev => ev.Attendances)
             .HasForeignKey(a => a.EventId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(a => a.Player)
             .WithMany(pp => pp.Attendances)
             .HasForeignKey(a => a.PlayerId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(a => a.RecordedByStaff)
             .WithMany(s => s.AttendanceRecords)
             .HasForeignKey(a => a.RecordedByStaffId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Match ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Match>(e =>
        {
            e.HasKey(m => m.MatchId);

            e.HasOne(m => m.Event)
             .WithOne(ev => ev.Match)
             .HasForeignKey<Match>(m => m.EventId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(m => m.ScoreKeeperStaff)
             .WithMany()
             .HasForeignKey(m => m.ScoreKeeperStaffId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ── TeamGameStats ─────────────────────────────────────────────────────
        modelBuilder.Entity<TeamGameStats>(e =>
        {
            e.HasKey(tgs => tgs.ReportId);

            e.HasOne(tgs => tgs.Match)
             .WithOne(m => m.TeamGameStats)
             .HasForeignKey<TeamGameStats>(tgs => tgs.MatchId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(tgs => tgs.AnalystStaff)
             .WithMany(s => s.AnalyzedStats)
             .HasForeignKey(tgs => tgs.AnalystStaffId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ── PlayerGameStats ───────────────────────────────────────────────────
        modelBuilder.Entity<PlayerGameStats>(e =>
        {
            e.HasKey(pgs => pgs.GameStatId);

            e.HasOne(pgs => pgs.Match)
             .WithMany(m => m.PlayerGameStats)
             .HasForeignKey(pgs => pgs.MatchId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pgs => pgs.Player)
             .WithMany(pp => pp.GameStats)
             .HasForeignKey(pgs => pgs.PlayerId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── TeamCumulativeStats ───────────────────────────────────────────────
        modelBuilder.Entity<TeamCumulativeStats>(e =>
        {
            e.HasKey(tcs => tcs.TeamStatId);

            e.HasOne(tcs => tcs.Team)
             .WithMany(t => t.CumulativeStats)
             .HasForeignKey(tcs => tcs.TeamId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── PlayerCumulativeStats ─────────────────────────────────────────────
        modelBuilder.Entity<PlayerCumulativeStats>(e =>
        {
            e.HasKey(pcs => pcs.PlayerStatId);

            e.HasOne(pcs => pcs.Player)
             .WithMany(pp => pp.CumulativeStats)
             .HasForeignKey(pcs => pcs.PlayerId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── AiLineupSuggestion ────────────────────────────────────────────────
        modelBuilder.Entity<AiLineupSuggestion>(e =>
        {
            e.HasKey(ai => ai.SuggestionId);

            e.HasOne(ai => ai.Match)
             .WithMany(m => m.AiLineupSuggestions)
             .HasForeignKey(ai => ai.MatchId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── MedicalRecord ─────────────────────────────────────────────────────
        modelBuilder.Entity<MedicalRecord>(e =>
        {
            e.HasKey(mr => mr.RecordId);

            e.HasOne(mr => mr.Player)
             .WithMany(pp => pp.MedicalRecords)
             .HasForeignKey(mr => mr.PlayerId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(mr => mr.DoctorStaff)
             .WithMany(s => s.MedicalRecords)
             .HasForeignKey(mr => mr.DoctorStaffId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── FitnessRecord ─────────────────────────────────────────────────────
        modelBuilder.Entity<FitnessRecord>(e =>
        {
            e.HasKey(fr => fr.FitnessId);

            e.HasOne(fr => fr.Player)
             .WithMany(pp => pp.FitnessRecords)
             .HasForeignKey(fr => fr.PlayerId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(fr => fr.FitnessStaff)
             .WithMany(s => s.FitnessRecords)
             .HasForeignKey(fr => fr.FitnessStaffId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Message ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Message>(e =>
        {
            e.HasKey(msg => msg.MessageId);
        });

        // ── Announcement ──────────────────────────────────────────────────────
        modelBuilder.Entity<Announcement>(e =>
        {
            e.HasKey(ann => ann.AnnouncementId);

            e.HasOne(ann => ann.Team)
             .WithMany(t => t.Announcements)
             .HasForeignKey(ann => ann.TeamId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(ann => ann.Creator)
             .WithMany(u => u.Announcements)
             .HasForeignKey(ann => ann.CreatorUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Seed default roles ────────────────────────────────────────────────
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, Name = "Admin",   Description = "System administrator" },
            new Role { RoleId = 2, Name = "Manager", Description = "Team manager" },
            new Role { RoleId = 3, Name = "Coach",   Description = "Head or assistant coach" },
            new Role { RoleId = 4, Name = "Player",  Description = "Registered player" },
            new Role { RoleId = 5, Name = "Medic",   Description = "Medical staff" },
            new Role { RoleId = 6, Name = "Analyst", Description = "Performance analyst" }
        );
    }
}
