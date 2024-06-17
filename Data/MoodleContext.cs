using System;
#nullable disable
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EF_WebApi.Models;

namespace EF_WebApi.Data;

public partial class MoodleContext : DbContext
{
    public MoodleContext(DbContextOptions<MoodleContext> options)
        : base(options)
    {
    }

    public  DbSet<MdlUser> mdl_user { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder
    //         .UseCollation("utf8mb4_unicode_ci")
    //         .HasCharSet("utf8mb4");

    //     modelBuilder.Entity<MdlUser>(entity =>
    //     {
    //         entity.HasKey(e => e.Id).HasName("PRIMARY");

    //         entity.ToTable("mdl_user", tb => tb.HasComment("One record for each person"));

    //         entity.HasIndex(e => e.Alternatename, "mdl_user_alt_ix");

    //         entity.HasIndex(e => e.Auth, "mdl_user_aut_ix");

    //         entity.HasIndex(e => e.City, "mdl_user_cit_ix");

    //         entity.HasIndex(e => e.Confirmed, "mdl_user_con_ix");

    //         entity.HasIndex(e => e.Country, "mdl_user_cou_ix");

    //         entity.HasIndex(e => e.Deleted, "mdl_user_del_ix");

    //         entity.HasIndex(e => e.Email, "mdl_user_ema_ix");

    //         entity.HasIndex(e => e.Firstnamephonetic, "mdl_user_fir2_ix");

    //         entity.HasIndex(e => e.Firstname, "mdl_user_fir_ix");

    //         entity.HasIndex(e => e.Idnumber, "mdl_user_idn_ix");

    //         entity.HasIndex(e => e.Lastaccess, "mdl_user_las2_ix");

    //         entity.HasIndex(e => e.Lastnamephonetic, "mdl_user_las3_ix");

    //         entity.HasIndex(e => e.Lastname, "mdl_user_las_ix");

    //         entity.HasIndex(e => e.Middlename, "mdl_user_mid_ix");

    //         entity.HasIndex(e => new { e.Mnethostid, e.Username }, "mdl_user_mneuse_uix").IsUnique();

    //         entity.Property(e => e.Id).HasColumnName("id");
    //         entity.Property(e => e.Address)
    //             .HasMaxLength(255)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("address");
    //         entity.Property(e => e.Alternatename).HasColumnName("alternatename");
    //         entity.Property(e => e.Auth)
    //             .HasMaxLength(20)
    //             .HasDefaultValueSql("'manual'")
    //             .HasColumnName("auth");
    //         entity.Property(e => e.Autosubscribe)
    //             .IsRequired()
    //             .HasDefaultValueSql("'1'")
    //             .HasColumnName("autosubscribe");
    //         entity.Property(e => e.Calendartype)
    //             .HasMaxLength(30)
    //             .HasDefaultValueSql("'gregorian'")
    //             .HasColumnName("calendartype");
    //         entity.Property(e => e.City)
    //             .HasMaxLength(120)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("city");
    //         entity.Property(e => e.Confirmed).HasColumnName("confirmed");
    //         entity.Property(e => e.Country)
    //             .HasMaxLength(2)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("country");
    //         entity.Property(e => e.Currentlogin).HasColumnName("currentlogin");
    //         entity.Property(e => e.Deleted).HasColumnName("deleted");
    //         entity.Property(e => e.Department)
    //             .HasMaxLength(255)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("department");
    //         entity.Property(e => e.Description).HasColumnName("description");
    //         entity.Property(e => e.Descriptionformat)
    //             .HasDefaultValueSql("'1'")
    //             .HasColumnName("descriptionformat");
    //         entity.Property(e => e.Email)
    //             .HasMaxLength(100)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("email");
    //         entity.Property(e => e.Emailstop).HasColumnName("emailstop");
    //         entity.Property(e => e.Firstaccess).HasColumnName("firstaccess");
    //         entity.Property(e => e.Firstname)
    //             .HasMaxLength(100)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("firstname");
    //         entity.Property(e => e.Firstnamephonetic).HasColumnName("firstnamephonetic");
    //         entity.Property(e => e.Idnumber)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("idnumber");
    //         entity.Property(e => e.Imagealt)
    //             .HasMaxLength(255)
    //             .HasColumnName("imagealt");
    //         entity.Property(e => e.Institution)
    //             .HasMaxLength(255)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("institution");
    //         entity.Property(e => e.Lang)
    //             .HasMaxLength(30)
    //             .HasDefaultValueSql("'en'")
    //             .HasColumnName("lang");
    //         entity.Property(e => e.Lastaccess).HasColumnName("lastaccess");
    //         entity.Property(e => e.Lastip)
    //             .HasMaxLength(45)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("lastip");
    //         entity.Property(e => e.Lastlogin).HasColumnName("lastlogin");
    //         entity.Property(e => e.Lastname)
    //             .HasMaxLength(100)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("lastname");
    //         entity.Property(e => e.Lastnamephonetic).HasColumnName("lastnamephonetic");
    //         entity.Property(e => e.Maildigest).HasColumnName("maildigest");
    //         entity.Property(e => e.Maildisplay)
    //             .HasDefaultValueSql("'2'")
    //             .HasColumnName("maildisplay");
    //         entity.Property(e => e.Mailformat)
    //             .IsRequired()
    //             .HasDefaultValueSql("'1'")
    //             .HasColumnName("mailformat");
    //         entity.Property(e => e.Middlename).HasColumnName("middlename");
    //         entity.Property(e => e.Mnethostid).HasColumnName("mnethostid");
    //         entity.Property(e => e.Moodlenetprofile)
    //             .HasMaxLength(255)
    //             .HasColumnName("moodlenetprofile");
    //         entity.Property(e => e.Password)
    //             .HasMaxLength(255)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("password");
    //         entity.Property(e => e.Phone1)
    //             .HasMaxLength(20)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("phone1");
    //         entity.Property(e => e.Phone2)
    //             .HasMaxLength(20)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("phone2");
    //         entity.Property(e => e.Picture).HasColumnName("picture");
    //         entity.Property(e => e.Policyagreed).HasColumnName("policyagreed");
    //         entity.Property(e => e.Secret)
    //             .HasMaxLength(15)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("secret");
    //         entity.Property(e => e.Suspended).HasColumnName("suspended");
    //         entity.Property(e => e.Theme)
    //             .HasMaxLength(50)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("theme");
    //         entity.Property(e => e.Timecreated).HasColumnName("timecreated");
    //         entity.Property(e => e.Timemodified).HasColumnName("timemodified");
    //         entity.Property(e => e.Timezone)
    //             .HasMaxLength(100)
    //             .HasDefaultValueSql("'99'")
    //             .HasColumnName("timezone");
    //         entity.Property(e => e.Trackforums).HasColumnName("trackforums");
    //         entity.Property(e => e.Trustbitmask).HasColumnName("trustbitmask");
    //         entity.Property(e => e.Username)
    //             .HasMaxLength(100)
    //             .HasDefaultValueSql("''")
    //             .HasColumnName("username");
    //     });

    //     OnModelCreatingPartial(modelBuilder);
    // }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
