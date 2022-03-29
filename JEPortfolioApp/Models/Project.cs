namespace JEPortfolioApp.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string FilePath { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public Project()
        {

        }
    }
}
