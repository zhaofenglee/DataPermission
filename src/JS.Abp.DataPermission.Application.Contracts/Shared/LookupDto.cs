namespace JS.Abp.DataPermission.Shared
{
    public class LookupDto<TKey>
    {
        public TKey Id { get; set; }

        public string DisplayName { get; set; }
    }
}