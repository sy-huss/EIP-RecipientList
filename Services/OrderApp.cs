namespace RecipientList.Services
{
    public class OrderApp : IOrderQueue
    {
        public static string _order;

        public void CreateOrders()
        {
            _order = @"
{
'first_name':'Judd',
'last_name':'Dhennin',
'email':'jdhennin0@ask.com',
'gender':'Male',
'ip_address':'186.7.174.216',
'order_id':'173533074-4'
}";
        }
    }
}