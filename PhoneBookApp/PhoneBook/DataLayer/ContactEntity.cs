[Table("Contacts")]
public class ContactEntity
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }

    [Required]
    [Column("FirstName")]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [Column("LastName")]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [Column("Phone")]
    [MaxLength(15)]
    public string Phone { get; set; }

    [Column("Address")]
    [MaxLength(100)]
    public string Address { get; set; }
}