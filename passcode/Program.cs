using System;
using System.Collections.Generic;
using System.Linq;

namespace passcode
{
    // A B C
    // D E F
    // G H I

    // 0 1 2
    // 3 4 5
    // 6 7 8
    
    static class Program
    {
        static void Main(string[] args)
        {
            int initialNode = 4;
            int length = 8;
            RouteFinder finder = new RouteFinder(initialNode, length);
            Console.WriteLine(finder.Find().Count);
        }
    }
    
    public class Route
    {
        private static HashSet<int> AvailableNodes { get; set; }
        private static List<List<int>> SpecialRoutes { get; set; }
        private int CurrentNode { get; set; }
        private HashSet<int> VisitedNodes { get; set; }

        static Route()
        {
            AvailableNodes = new HashSet<int>(9);
            for (int i = 0; i < 9; i++)
            {
                AvailableNodes.Add(i);
            }

            SpecialRoutes = new List<List<int>>(8)
            {
                new() { 0, 2 },
                new() { 2, 8 },
                new() { 6, 8 },
                new() { 0, 6 },
                new() { 1, 7 },
                new() { 3, 5 },
                new() { 0, 8 },
                new() { 2, 6 }
            };
        }

        public Route(int currentNode, HashSet<int> visitedNodes = null)
        {
            CurrentNode = currentNode;
            VisitedNodes = visitedNodes ?? new HashSet<int>();
            VisitedNodes.Add(currentNode);
        }

        public List<Route> GetOneStepLongerRoutes()
        {
            List<Route> routes = new List<Route>(8);

            foreach (int availableNode in AvailableNodes)
            {
                if (!IsMoveLegal(availableNode))
                {
                    continue;
                }

                Route route = new Route(availableNode, VisitedNodes.ToHashSet());
                routes.Add(route);
            }

            return routes;
        }

        private bool IsMoveLegal(int nextNode)
        {
            if (VisitedNodes.Contains(nextNode))
            {
                return false;
            }

            bool isSpecialRoute = SpecialRoutes.Any(x => x.Contains(CurrentNode) && x.Contains(nextNode));
            if (isSpecialRoute)
            {
                int dependantNode = (CurrentNode + nextNode) / 2;
                if (!VisitedNodes.Contains(dependantNode))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class RouteFinder
    {
        private int InitialNode { get; set; }
        private int Length { get; set; }

        public RouteFinder(int initialNode, int length)
        {
            InitialNode = initialNode;
            Length = length;
        }

        public List<Route> Find()
        {
            int estimatedCollectionSize = Helper.Factorial(Length);
            List<Route> routes = new List<Route>(estimatedCollectionSize);

            if (Length == 0)
            {
                return routes;
            }

            routes.Add(new Route(InitialNode));

            for (int currentLength = 1; currentLength < Length; currentLength++)
            {
                List<Route> currentPassRoutes = new List<Route>(routes.Count + 8);

                foreach (Route route in routes)
                {
                    currentPassRoutes.AddRange(route.GetOneStepLongerRoutes());
                }

                routes = currentPassRoutes;
            }

            return routes;
        }
    }
    
    public static class Helper
    {
        public static int Factorial(int number)
        {
            int result = 1;
            for (int i = 2; i <= number; i++)
            {
                result *= i;
            }

            return result;
        }
    }
}