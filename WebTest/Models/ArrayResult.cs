namespace WebTest.Models
{
    public class ArrayResult<T>
    {
        public ArrayResult(IEnumerable<T> items)
        {
            Result = items;
        }
        public ArrayResult()
        {
        }
        public IEnumerable<T> Result { get; set; }
    }
}
