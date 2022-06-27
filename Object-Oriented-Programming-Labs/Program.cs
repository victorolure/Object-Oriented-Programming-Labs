



VendingMachine VendingMachine = new VendingMachine("VM001");

//VendingMachine v2 = new VendingMachine("102");

Console.WriteLine("here "+ VendingMachine.SerialNumber);

Product Juice = new Product("Juice", 2, "A102");
Product KetchUp = new Product("Ketchup", 4, "A103");

VendingMachine.StockItem(Juice, 5);
VendingMachine.StockItem(KetchUp, 6);
Console.WriteLine(VendingMachine.StockItem(Juice, 6));

VendingMachine.StockFLoat(20, 5);
VendingMachine.StockFLoat(10, 5);
VendingMachine.StockFLoat(5, 5);
VendingMachine.StockFLoat(2, 5);
VendingMachine.StockFLoat(1, 5);


List<int> money = new List<int> { 5, 5, 5 };
string vend = VendingMachine.VendItem("A102", money);
Console.WriteLine(vend);

VendingMachine.ExposeChangeFloat();
VendingMachine.ExposeInventory();









class VendingMachine
{
    public static int SerialNumber { get; set; } = 0;

    public string BarCode {get;}

    //public string BarCode { get { return _barCode; } set { _barCode = value;  } }

    private Dictionary<int, int> _changeFloat { get; set; }

    public void ExposeChangeFloat()
    {
        Dictionary<int, int> change = new Dictionary<int, int>();
        foreach(KeyValuePair<int, int>kvp in _changeFloat)
        {
            change.Add(kvp.Key, kvp.Value);
        }
        foreach(KeyValuePair<int, int> kvp in change)
        {
            Console.WriteLine(kvp.Key + " - " + kvp.Value);
        }
    }
    

    public Dictionary<Product, int> _inventory { get; set; }

    public void ExposeInventory()
    {
        Dictionary<Product, int> inventory = new Dictionary<Product, int>();
        foreach(KeyValuePair<Product, int>kvp in _inventory)
        {
            inventory.Add(kvp.Key, kvp.Value);
        }
        foreach (KeyValuePair<Product, int> kvp in inventory)
        {
            Console.WriteLine($"Product Name:{kvp.Key.Name}, Quantity: {kvp.Value}");
        }

    }

    public string StockItem (Product product, int quantity)
    {
        if (_inventory.ContainsKey(product))
        {
            _inventory[product]+=quantity;
        }
        else
        {
            _inventory.Add(product, quantity);
        }
        return $"Product: {product.Name}\nCode: {product.Code} \nUpdated Quantity: {_inventory[product]} ";
    }

    public void StockFLoat(int moneyDenomination, int quantity)
    {
        if (_changeFloat.ContainsKey(moneyDenomination))
        {
            _changeFloat[moneyDenomination]+= quantity;
        }
        else
        {
            _changeFloat.Add(moneyDenomination, quantity);
        }
   
    }

    public Product GetProduct (string code)
    {
        Product product;
        foreach(KeyValuePair<Product, int> item in _inventory)
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
        List<int> changeList = GetChangeList(_changeFloat);
        string message= "";
        Product product = GetProduct(code);
        
        int coinsToExtract;
        if(product!= null)
        {
            int change = moneyTotal - product.Price;
            if (GetTotalChange(_changeFloat) >= change)
            {
                if (_inventory[product] >= 1)
                {
                    if (moneyTotal >= product.Price)
                    {
                        _inventory[product] -= 1;    
                        foreach(int i in changeList)
                        {
                            if(change >= i)
                            {
                                coinsToExtract = change / i;
                                if (_changeFloat[i] >= coinsToExtract)
                                {
                                    _changeFloat[i]-= coinsToExtract;
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

    public VendingMachine(string barCode)
    {
        SerialNumber++;
        _inventory = new Dictionary<Product, int>();
        _changeFloat = new Dictionary<int, int>();
        BarCode= barCode;   
        
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



