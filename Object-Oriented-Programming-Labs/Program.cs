Room room1 = new Regular();
Client Olu = new Client("Olu", 1111222233334444);
Room room2 = new Premium();
Client Dami = new Client("Dami", 2222444455556666);
Room premium101 = new Premium();

Hotel.ReserveRoom(Olu, room1);
Hotel.ReserveRoom(Dami, room2);
Hotel.Rooms.Add(premium101);

try
{
    Hotel.ReserveSpecialRoomType(Olu, 5);
}catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}



static class Hotel
{
    public static string Name { get; set; } = "HouseEleven";
    public static string Address { get; set; } = "130 Henlow Bay";
    
    public static List<Room> Rooms { get; set; } = new List<Room>();
    public static int NumberOfClients { get; set; } = 0;
    public static HashSet<Client> Clients { get; set; } = new HashSet<Client>();

    public static HashSet<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
    public static Client RegisterClient(string name, long carddetails)
    {
        Client newClient = new Client(name, carddetails);
        Clients.Add(newClient);
        NumberOfClients++;
        return newClient;
    }
    public static void ClearHotel()
    {
        foreach(Client client in Clients)
        {
            Clients.Remove(client);
        }
    }
    public static void VacateReservations(Client customer)
    {
        foreach(Client client in Clients)
        {
            if (client.Name == customer.Name)
            {
                client.Reservations.Clear();

                
            }
        }
    }
    public static Reservation ReserveRoom(Client client, Room room)
    {
        Reservation newReservation = new Reservation(client, room);
        newReservation.isCurrent = true;
        newReservation.ReservationID = Reservations.Count + 1;
        Reservations.Add(newReservation);
        room.Reservations.Add(newReservation);
        client.Reservations.Add(newReservation);
        room.Number = Rooms.Count + 1;
        room.Occupied = true;
        Rooms.Add(room);
        return newReservation;
    }


    public static Reservation ReserveSpecialRoomType(Client client, int minimumCapacity)
    {
        Premium s;
        Regular v;

        foreach(Room unit in Rooms)
        {
            if (unit.GetType().FullName == "Premium" && unit.Occupied == false)
            {
                s = (Premium)unit;
                if (s.Capacity >= minimumCapacity)
                {
                    Reservation newReservation = new Reservation(client, s);
                    newReservation.ReservationID = Reservations.Count + 1;
                    newReservation.Occupants = minimumCapacity;
                    newReservation.isCurrent = true;
                    Reservations.Add(newReservation);
                    client.Reservations.Add(newReservation);
                    Clients.Add(client);
                    s.Occupied = true;
                    s.Reservations.Add(newReservation);
                    return newReservation;
                }
            }else if(unit.GetType().FullName == "Regular" && unit.Occupied == false)
            {
                v = (Regular)unit;
                if(minimumCapacity <= 2)
                {
                    Reservation reservation = new Reservation(client, v);
                    reservation.ReservationID = Reservations.Count + 1;
                    reservation.Occupants = minimumCapacity;
                    reservation.isCurrent = true;
                    Reservations.Add(reservation);
                    client.Reservations.Add(reservation);
                    Clients.Add(client);
                    v.Occupied = true;
                    v.Reservations.Add(reservation);
                    return reservation;
                }
            }
           
            
        }
        throw new Exception("Sorry,there are no rooms with the specified capacity");
    }


}

class Room
{
    public int Number { get; set; }
    public int Capacity { get; set; }
    public bool Occupied { get; set; } = false;
    public ICollection<Reservation> Reservations { get; set;}

    public Room ()
    {
        Reservations = new HashSet<Reservation>();            
    }

}

class Regular: Room
{
    public int Capacity { get; set; } = 2;
    public Regular()
    {

        Reservations = new HashSet<Reservation>();
    }
}

class Premium: Room
{
    public int Capacity { get; set; } = 4;
    public Premium()
    {
       
        Reservations= new HashSet<Reservation>();
    }
}





class Client
{
    public string Name { get; set; }
    private long _creditCard { get; set; }

    public ICollection<Reservation> Reservations { get; set;}
    public Client(string name, long creditCard)
    {
        Name = name;
        Reservations = new HashSet<Reservation>();
        _creditCard = creditCard;
    }

}

class Reservation
{
    public DateTime Date { get; set; }= new DateTime();
    public int Occupants { get; set; }
    public bool isCurrent { get; set; } = false;
    public Room Room { get; set; }
    public int ReservationID { get; set; }
    public Client Client { get; set; }
    public Reservation (Client client, Room room)
    {
        Client = client;
        Room = room;
    }
}