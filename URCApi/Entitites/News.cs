namespace URCApi.Entitites
{
    public class News
    {
        public int Id { get; set; }
        public string NewsHeader { get; set; }
        public string NewsText { get; set; }
        public string FileName { get; set; } 
        public string FilePath { get; set; }
        public int ImageId { get; set; }

    }
}
