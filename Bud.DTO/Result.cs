namespace Bud.DTO
{
    using System.Collections.Generic;

    public class Result<T>
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public T Item { get; set; }
    }
}