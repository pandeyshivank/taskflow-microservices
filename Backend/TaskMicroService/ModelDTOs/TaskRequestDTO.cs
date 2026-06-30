namespace TaskMicroService.ModelDTOs
{
    public class TaskRequestDTO
    {
      
            public Guid Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public DateTime DueDate { get; set; }
            public string Status { get; set; } = string.Empty;
            public Guid UserId { get; set; }
        
    }
}
