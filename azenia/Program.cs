namespace Viagogo
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class Solution
    {
        static void Main(string[] args)
        {
            var events = new List<Event>{
                new Event{ Name = "Phantom of the Opera", City = "New York"},
                new Event{ Name = "Metallica", City = "Los Angeles"},
                new Event{ Name = "Metallica", City = "New York"},
                new Event{ Name = "Metallica", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "New York"},
                new Event{ Name = "LadyGaGa", City = "Boston"},
                new Event{ Name = "LadyGaGa", City = "Chicago"},
                new Event{ Name = "LadyGaGa", City = "San Francisco"},
                new Event{ Name = "LadyGaGa", City = "Washington"}
            };
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };

            // 1. TASK
            // find out all events that are in cities of customer
            // then add to email.

            var customerEvents = GetEventsInCustomerCity(customer.City, events);
            Console.WriteLine("Events in customer city");
            foreach (var item in customerEvents)
            {
                AddToEmail(customer, item);
            }
            Console.WriteLine("=====================");

            /*
                To improve the above code we can collate all the events in an array
                and send to the customer as one email instead of looping through all the events 
            */


            //2. TASK
            // find out all events that are in cities of customer and nearest cities
            // then add to email.

            var nearestEvents = GetNearestEventToCustomers(customer.City, events);
            Console.WriteLine("Events nearest to customer city");
            foreach (var item in nearestEvents)
            {
                AddToEmail(customer, item);
            }
            Console.WriteLine("================");


            //3. TASK
            /*
             *  refer to comment on GetNearestEventToCustomers() for optimization solution
            */


            //4. TASK
            /*
             * We can wrap GetNearestEventToCustomers() method with a try catch and on exception 
             * we call GetEventsInCustomerCity() to return only event in customer city
            */


            //5. TASK
            var eventPricing = SortNearestEventByPrice(nearestEvents);
            Console.WriteLine("Events nearest to customer city sorted by price");
            foreach (var item in eventPricing)
            {
                AddToEmail(customer, item);
            }
            Console.WriteLine("================");


            //6. TASK
            /*
            *To verify if what has been is correct we run test against the test data provided and also come up with a Unit Test 
            *that assert the given result with the expected result
            */
        }

        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)

        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        }
        static int GetDistance(string fromCity, string toCity)
        {
            return AlphebiticalDistance(fromCity, toCity);
        }
        private static int AlphebiticalDistance(string s, string t)
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }

        private static IEnumerable<Event> GetEventsInCustomerCity(string customerCity, List<Event> events)
        {
            var query = from result in events
                        where result.City.ToLower().Trim().Equals(customerCity?.ToLower().Trim())
                        select result;
            return query;
        }


        private static IEnumerable<Event> GetNearestEventToCustomers(string customerCity, List<Event> events)
        {
            //For optimization we can use a caching database like redis to store the distance against the customer city that calls the method
            //if the customerCity exist in the caching database we return the cached distance else ...
            Dictionary<Event, int> eventsDistance = new Dictionary<Event, int>();
            foreach (var item in events)
            {
                var distance = GetDistance(customerCity, item.City);
                eventsDistance.Add(item, distance);
            }

            var result = (from item in eventsDistance orderby item.Value
            ascending select item.Key).Take(5);

            // Save result to caching database with customerCity as the key
            return result;
        }

        private static IEnumerable<Event> SortNearestEventByPrice(IEnumerable<Event> events)
        {
            Dictionary<Event, int> eventsPricing = new Dictionary<Event, int>();
            foreach (var item in events)
            {
                var eventPrice = GetPrice(item);
                eventsPricing.Add(item, eventPrice);
            }

            var result = (from item in eventsPricing
                          orderby item.Value
                          ascending
                          select item.Key).Take(5);
            return result;
        }

    }
}

/*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/