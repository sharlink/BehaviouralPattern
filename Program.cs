using System;
using System.Collections.Generic;

namespace BehaviouralPattern
{
    // The Handler interface declares a method for building the chain of
    // handlers. It also declares a method for executing a request.
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        object Handle(object request);
    }

    // The default chaining behavior can be implemented inside a base handler
    // class.
    abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            this._nextHandler = handler;

            // Returning a handler from here will let us link handlers in a
            // convenient way like this:
            // amenities1.SetNext(amenities2).SetNext(amenities3);
            return handler;
        }

        public virtual object Handle(object request)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }

    class AmenitiesHandler1 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if ((request as string) == "Gymnasium")
            {
                return $"Amenities1: I'll avail the {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class AmenitiesHandler2 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "Pool")
            {
                return $"Amenities2: I'll avail the {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class AmenitiesHandler3 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "Buffet")
            {
                return $"Amenities3: I'll avail the {request.ToString()}.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class Client
    {
        // The client code is usually suited to work with a single handler. In
        // most cases, it is not even aware that the handler is part of a chain.
        public static void ClientCode(AbstractHandler handler)
        {
            foreach (var service in new List<string> { "Gymnasium", "Pool", "Buffet" })
            {
                Console.WriteLine($"Client: Which  {service}?");

                 var result = handler.Handle(service);

                if (result != null)
                {
                    Console.Write($"   {result}");
                }
                else
                {
                    Console.WriteLine($"   {service} was unavailable.");
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // The other part of the client code constructs the actual chain.
            var amenities1 = new AmenitiesHandler1();
            var amenities2 = new AmenitiesHandler2();
            var amenities3 = new AmenitiesHandler3();

            amenities1.SetNext(amenities2).SetNext(amenities3);

            // The client should be able to send a request to any handler, not
            // just the first one in the chain.
            Console.WriteLine("Chain: Amenities1 > Amenities2 > Amenities3\n");
            Client.ClientCode(amenities1);
            Console.WriteLine();

            Console.WriteLine("Subchain: Amenities2 > Amenities3\n");
            Client.ClientCode(amenities2);
        }
    }
}
