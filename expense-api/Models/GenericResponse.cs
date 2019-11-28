using System;

namespace expense_api.Models
{
    public class GenericResponse
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
