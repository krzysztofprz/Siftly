namespace Siftly.UnitTests.Model
{
    public class User
    {
        public int Id { get; set; }
        public int? SubscriptionId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Verified { get; set; }
        public bool? HasLogged { get; set; }
        public Address Address { get; set; }
    }
}