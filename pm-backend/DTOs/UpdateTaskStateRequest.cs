using pm_backend.Models;

namespace pm_backend.DTOs
{
    public class UpdateTaskStateRequest
    {
        public TaskState State { get; set; }
    }
}
