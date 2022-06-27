



VendingMachine VendingMachine = new VendingMachine(1234);

Product Juice = new Product("Juice", 2, "A102");

VendingMachine.StockItem(Juice, 5);
Console.WriteLine(VendingMachine.StockItem(Juice, 6));
foreach (KeyValuePair<Product, int> item in VendingMachine.Inventory)
{
    Console.WriteLine(VendingMachine.Inventory[item.Key]);
}
VendingMachine.StockFLoat(20, 5);
VendingMachine.StockFLoat(10, 5);
VendingMachine.StockFLoat(5, 5);
VendingMachine.StockFLoat(2, 5);
VendingMachine.StockFLoat(1, 5);


List<int> money = new List<int> { 5, 5, 5 };
string vend = VendingMachine.VendItem("A102", money);
Console.WriteLine(vend);




class VendingMachine
{
    public int SerialNumber { get; set; } 

    public Dictionary<int, int> ChangeFloat { get; set; }

    public Dictionary<Product, int> Inventory { get; set; }

    

    public VendingMachine(int number)
    {
        SerialNumber = number;
        Inventory = new Dictionary<Product, int>();
        ChangeFloat = new Dictionary<int, int>();
    }

    public string StockItem (Product product, int quantity)
    {
        if (Inventory.ContainsKey(product))
        {
            Inventory[product]+=quantity;
        }
        else
        {
            Inventory.Add(product, quantity);
        }
        return $"Product: {product.Name}\nCode: {product.Code} \nUpdated Quantity: {Inventory[product]} ";
    }

    public void StockFLoat(int moneyDenomination, int quantity)
    {
        if (ChangeFloat.ContainsKey(moneyDenomination))
        {
            ChangeFloat[moneyDenomination]+= quantity;
        }
        else
        {
            ChangeFloat.Add(moneyDenomination, quantity);
        }
   
    }

    public Product GetProduct (string code)
    {
        Product product;
        foreach(KeyValuePair<Product, int> item in Inventory)
        {
            if(item.Key.Code== code)
            {
                product = item.Key;
                return product;
            }
        }
        return null;
    }

    public string VendItem(string code, List<int> money)
    {
        int moneyTotal = money.Sum();
        List<int> changeList = GetChangeList(ChangeFloat);
        string message= "";
        Product product = GetProduct(code);
        
        int coinsToExtract;
        if(product!= null)
        {
            int change = moneyTotal - product.Price;
            if (GetTotalChange(ChangeFloat) >= change)
            {
                if (Inventory[product] >= 1)
                {
                    if (moneyTotal >= product.Price)
                    {
                        Inventory[product] -= 1;    
                        foreach(int i in changeList)
                        {
                            if(change >= i)
                            {
                                coinsToExtract = change / i;
                                if (ChangeFloat[i] >= coinsToExtract)
                                {
                                    ChangeFloat[i]-= coinsToExtract;
                                }
                                

                                


                            }
                        }

                        message += $"Please enjoy your {product.Name} and take your change of {change}";
                    }
                    else
                    {
                        message += $"Error: Insufficient money provided";
                    }
                }
                else
                {
                    message += $"Error: Item is out of stock";
                }

            }
            else
            {
                message += $"Error: not enough change in the system";
            }
            
        }
        else
        {
            message += $"Error: no item with code: '{code}' ";
        }
        return message;

    }

    public int GetTotalChange(Dictionary<int, int> wallet)
    {
        int total = 0;
        foreach(KeyValuePair<int,int> kvp in wallet)
        {
            total+= kvp.Key* kvp.Value;
        }
        return total;
    }
    public List<int>GetChangeList(Dictionary<int, int> wallet)
    {
        List <int>changeList = new List <int>();
        foreach(KeyValuePair<int,int> kvp in wallet)
        {
            changeList.Add(kvp.Key);
        }
        return changeList;
    }
    


}


class Product
{
    public string Name { get; set; } 
    public int Price { get; set; }

    public string Code { get; set; }

    public Product(string name, int price, string code)
    {
        Name = name;
        Price = price;  
        Code = code;    
    }

}



