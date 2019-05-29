namespace EthereumAPI.Form
{
    public class Transaction
    {
        public string SenderPrivateKey {get; set;} 
        public string RecieverAddress {get; set;}
        public string SenderAddress{get;set;}
        public decimal Amount {get;set;}
        public decimal GasPrice {get;set;}
    }
}