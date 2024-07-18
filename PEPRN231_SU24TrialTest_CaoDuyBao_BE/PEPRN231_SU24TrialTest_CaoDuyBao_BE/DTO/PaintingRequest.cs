namespace PEPRN231_SU24TrialTest_CaoDuyBao_BE.DTO
{
    public class PaintingRequest
    {
        public string PaintingName { get; set; } = null!;
        public string? PaintingDescription { get; set; }
        public string? PaintingAuthor { get; set; }
        public decimal? Price { get; set; }
        public int? PublishYear { get; set; }

        public string? StyleId { get; set; }
    }
}