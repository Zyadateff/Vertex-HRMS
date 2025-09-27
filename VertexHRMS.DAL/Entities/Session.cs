using System.ComponentModel.DataAnnotations;

namespace VertexHRMS.DAL.Entities
{
    public class Session
    {
        [Key]
        [MaxLength(500)]
        public string Id { get; set; } = string.Empty;
        [Required]
        public byte[] Value { get; set; } = Array.Empty<byte>();
        [Required]
        public DateTimeOffset ExpiresAtTime { get; set; }
        public long? SlidingExpirationInSeconds { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
    }
}
