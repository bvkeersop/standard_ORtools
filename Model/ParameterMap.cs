using standard_ORtools.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace standard_ORtools.Service
{
    class ParameterMap
    {
        public int NumNodes { get; }
        public int NumArcs { get; }
        public int[] StartNodes { get; }
        public int[] EndNodes { get; }
        public int[] UnitCosts { get; }
        public int[] Capacities { get; }
        public int[] Supplies { get; }

        public ParameterMap(List<Consultant> consultants)
        {
            int nmbrOfConsultants = consultants.Count();
            int nmbrOfClients = consultants[0].TraveltimesInMinutes.Count();
            int endNodeStartId = nmbrOfConsultants + 1;

            NumNodes = GetNumberOfNodes(nmbrOfConsultants, nmbrOfClients);
            NumArcs = GetNumberOfArcs(nmbrOfConsultants, nmbrOfClients);
            StartNodes = GetStartNodes(consultants);
            EndNodes = GetEndNodes(endNodeStartId, nmbrOfClients, nmbrOfConsultants);
            UnitCosts = GetUnitCosts(consultants);
            Capacities = GetCapacities(NumNodes);
            Supplies = GetSupplies(nmbrOfConsultants, nmbrOfClients);
        }

        private int GetNumberOfNodes(int numberOfConsultants, int numberOfClients)
        {
            return numberOfConsultants + numberOfClients;
        }

        private int GetNumberOfArcs(int numberOfConsultants, int numberOfClients)
        {
            return numberOfConsultants * numberOfClients;
        }

        private int[] GetStartNodes(List<Consultant> consultants)
        {
            int numberOfConsultants = consultants.Count();
            int numberOfClients = consultants[0].TraveltimesInMinutes.Count();

            var startNodes = new List<int>();

            foreach(var consultant in consultants)
            {
                for(int j = 0; j < numberOfClients; j++)
                {
                    startNodes.Add(consultant.ConsultantId);
                }
            }

            return startNodes.ToArray();
        }

        private int[] GetEndNodes(int endNodeStartId, int numberOfClients, int numberOfConsultants)
        {
            var sequence = Enumerable.Range(endNodeStartId, numberOfClients);
            var endNodes = new List<int>();

            for(int i = 0; i < numberOfConsultants; i++)
            {
                endNodes.AddRange(sequence);
            }

            return endNodes.ToArray();
        }

        private int[] GetUnitCosts(List<Consultant> consultants)
        {
            var unitCosts = new List<int>();

            foreach(var consultant in consultants)
            {
                unitCosts.AddRange(consultant.TraveltimesInMinutes);
            }

            return unitCosts.ToArray();
        }

        private int[] GetCapacities(int numberOfNodes)
        {
            int capacityNumber = 1; //hardcoded for now
            int[] capacity = new int[numberOfNodes];

            for(int i = 0; i < numberOfNodes; i++)
            {
                capacity[i] = capacityNumber;
            }

            return capacity;
        }

        private int[] GetSupplies(int numberOfConsultants, int numberOfClients)
        {
            int[] supplyConsultants = new int[numberOfConsultants];
            int[] supplyClients = new int[numberOfClients];

            for(int i = 0; i < numberOfConsultants; i++)
            {
                supplyConsultants[i] = 1;
            }

            for (int i = 0; i < numberOfClients; i++)
            {
                supplyClients[i] = -1;
            }

            return supplyConsultants.Concat(supplyClients).ToArray();
        }

        private bool CompareWithNumberOfNodes(int size, int numberOfNodes)
        {
            return (size == numberOfNodes);
        }

        public void WriteDataToConsole()
        {
            Console.WriteLine("The following map is generated:");
            Console.WriteLine("NumNodes: {0}, Numarcs: {1}, StartNodes: {2}, EndNotes: {3}," +
                " UnitCosts: {4}, Capacity: {5}, Supplies: {6}",
                NumNodes, NumArcs, StartNodes.Count(), EndNodes.Count(),
                UnitCosts.Count(), Capacities.Count(), Supplies.Count());
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
