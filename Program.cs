using Google.OrTools.LinearSolver;
using Google.OrTools.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using standard_ORtools.Service;
using standard_ORtools.Model;
using System.Diagnostics;

namespace standard_ORtools
{
    class Program
    {
        private static MockDataGeneratorService mockDataGenerator;
        private static Stopwatch stopWatch;
        private static int CONSULTANTS = 30;
        private static int CLIENTS = 30;

        static void Main(string[] args)
        {
            var mocker = new MockDataGeneratorService();
            FlowParameterMap flowParameterMap = 
                new FlowParameterMap(mocker.GenerateRandomConsultantList(CONSULTANTS, CLIENTS));
            SolveMinCostFlow(flowParameterMap);
        }

        private static void TestLinearStuff()
        {
            Solver solver = Solver.CreateSolver("LinearExample", "GLOP_LINEAR_PROGRAMMING");

            Variable a = solver.MakeNumVar(0.0, double.PositiveInfinity, "a");
            Variable b = solver.MakeNumVar(0.0, double.PositiveInfinity, "b");
            Variable c = solver.MakeNumVar(0.0, double.PositiveInfinity, "b");

            int[][] timeMatrix =
                    {
                        new int[] {1,2,3},
                        new int[] {2,2,1},
                        new int[] {1,3,1}
                    };

            //supply and demand
            int[] sd = new int[] { 1, 1, 1 };

            //Set the variable (we want to minimize the travel time).

        }

        public static void StartProgram()
        {
            /*
            Console.WriteLine("Running test program, press enter to continue.");
            Console.ReadLine();
            RunLinearProgrammingExample("GLOP_LINEAR_PROGRAMMING");
            */

            Console.WriteLine("Program started");

            //Generate the consultants and clients.
            Console.WriteLine("Press any key to generate test data (consultants and clients).");
            Console.ReadKey();
            var consultantsList = GenerateConsultants();

            //Map the data so it's usable.
            Console.WriteLine("Press any key to start mapping the data to a useable format.");
            Console.ReadKey();
            var parameterMap = new FlowParameterMap(consultantsList);

            //Show data information
            parameterMap.WriteDataToConsole();
            //parameterMap.WriteDetailedDataToConsole();

            //Do calculations
            Console.WriteLine("Press any key to start solving the problem.");
            Console.ReadKey();
            SolveMinCostFlow(parameterMap);
        }

        private static List<Consultant> GenerateConsultants()
        {
            Console.WriteLine("Generating testdata.");
            mockDataGenerator = new MockDataGeneratorService();
            var consultantList = mockDataGenerator.GenerateRandomConsultantList(CONSULTANTS, CLIENTS);
            Console.WriteLine("Generated the following data:");
            Console.WriteLine("Number of consultants: {0}, number of companies: {1}, ConsultantId starts at: {2}",
                consultantList.Count(),
                consultantList[0].ClientAndTravelTime.Count,
                consultantList[0].ConsultantId);
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            return consultantList;
        }

        private static void RunLinearProgrammingExample(String solverType)
        {
            Solver solver = Solver.CreateSolver("LinearExample", solverType);
            // Create the variables x and y.
            Variable x = solver.MakeNumVar(0.0, 1.0, "x");
            Variable y = solver.MakeNumVar(0.0, 2.0, "y");
            // Create the objective function, x + y.
            Objective objective = solver.Objective();
            objective.SetCoefficient(x, 1);
            objective.SetCoefficient(y, 1);
            objective.SetMaximization();
            // Call the solver and display the results.
            solver.Solve();
            Console.WriteLine("Solution:");
            Console.WriteLine("x = " + x.SolutionValue());
            Console.WriteLine("y = " + y.SolutionValue());
            Console.ReadLine();
        }

        private static void SolveMinCostFlowHardcodedExample()
        {
            Console.WriteLine("Setting up problem.");
            //           A1      A2      A3      Supply
            // T1        1       2       3          1
            // T2        2       2       1          1
            // T2        1       3       1          1
            // Demand    1       1       1

            //total numer of consultants + clients
            int numNodes = 6;

            //numbers of arcs, this equals the clients * consultants
            int numArcs = 9;

            //start nodes are the nodes that represent consultants, they are represented with a simple int.
            //We have to represent a startnode for every end node.
            int[] startNodes = { 0, 0, 0, 1, 1, 1, 2, 2, 2 };

            //end nodes are the nodes that represent the clients, they are also represented with an int.
            //We have to represent an endnode for every startnode
            int[] endNodes = { 3, 4, 5, 3, 4, 5, 3, 4, 5 };

            //unitCosts are the cost from startNodes[i] to endNodes[i]. For example, from s-node 0 (T1) to e-node 2(A1) has a cost of 0 as seen in the table.
            //In our problem the unitCost is replced by the travelTime.
            int[] unitCosts = { 1, 2, 3, 2, 2, 1, 1, 3, 1 };

            //idk what capacities is
            int[] capacities = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            //supplies show the supply or demand of a node. consultant nodes always offer 1 consultant, and thus are 1.
            //A demand node depends on how many consultants the client requires. for now we will define this as 1.
            //supplies are shown by using a POSITIVE number. demands are a NEGATIVE number.
            int[] supplies = { 1, 1, 1, -1, -1, -1 };

            // Instantiate a SimpleMinCostFlow solver.
            MinCostFlow minCostFlow = new MinCostFlow();

            // Add each arc.
            Console.WriteLine("Adding arcs, amount: {0}", numArcs);
            for (int i = 0; i < numArcs; ++i)
            {
                Console.WriteLine("Adding arc: {0}", i);
                int arc = minCostFlow.AddArcWithCapacityAndUnitCost(startNodes[i], endNodes[i],
                                                     capacities[i], unitCosts[i]);
                //if (arc != i) throw new Exception("Internal error");
            }

            // Add node supplies.
            Console.WriteLine("Adding node supplies, amount {0}", numNodes);
            for (int i = 0; i < numNodes; ++i)
            {
                Console.WriteLine("Adding node supply: {0}", i);
                minCostFlow.SetNodeSupply(i, supplies[i]);
            }

            // Find the min cost flow.
            //Console.WriteLine("Solving min cost flow with " + numNodes + " nodes, and " +
            //                  numArcs + " arcs, source=" + source + ", sink=" + sink);


            // Find the min cost flow.
            //Console.WriteLine("Trying to solve mincostflow");
            int solveStatus = minCostFlow.Solve();

            if (solveStatus == MinCostFlow.OPTIMAL)
            {
                long optimalCost = minCostFlow.OptimalCost();
                Console.WriteLine("Minimum cost: " + optimalCost);
                Console.WriteLine("");
                Console.WriteLine(" Edge   Flow / Capacity  Cost");
                for (int i = 0; i < numArcs; ++i)
                {
                    long cost = minCostFlow.Flow(i) * minCostFlow.UnitCost(i);
                    Console.WriteLine(minCostFlow.Tail(i) + " -> " +
                                      minCostFlow.Head(i) + "  " +
                                      string.Format("{0,3}", minCostFlow.Flow(i)) + "  / " +
                                      string.Format("{0,3}", minCostFlow.Capacity(i)) + "       " +
                                      string.Format("{0,3}", cost));
                }
            }
            else
            {
                Console.WriteLine("Solving the min cost flow problem failed. Solver status: " +
                                  solveStatus);
            }

            //Wait and display solution
            Console.ReadKey();
        }

        private static void SolveMinCostFlow(FlowParameterMap parameterMap)
        {
            stopWatch = Stopwatch.StartNew();

            Console.WriteLine("Setting up problem.");
            MinCostFlow minCostFlow = new MinCostFlow();

            Console.WriteLine("Adding arcs, amount: {0}", parameterMap.NumArcs);
            for (int i = 0; i < parameterMap.NumArcs; ++i)
            {
                Console.WriteLine("Adding arc: {0}", i);
                int arc = minCostFlow.AddArcWithCapacityAndUnitCost(parameterMap.StartNodes[i], parameterMap.EndNodes[i],
                                                     parameterMap.Capacities[i], parameterMap.UnitCosts[i]);
                //if (arc != i) throw new Exception("Internal error");
            }

            //Add node supplies.
            Console.WriteLine("Adding node supplies, amount {0}", parameterMap.NumNodes);
            for (int i = 0; i < parameterMap.NumNodes; ++i)
            {
                //Console.WriteLine("Adding node supply: {0}", i);
                minCostFlow.SetNodeSupply(i, parameterMap.Supplies[i]);
            }

            // Find the min cost flow.
            Console.WriteLine("Trying to solve mincostflow");
            int solveStatus = minCostFlow.Solve();

            long optimalCost = 0;
            optimalCost = minCostFlow.OptimalCost();

            if (solveStatus == MinCostFlow.OPTIMAL)
            {

                for (int i = 0; i < parameterMap.NumArcs; ++i)
                    if (minCostFlow.Flow(i) > 0)
                    {
                        long cost = minCostFlow.Flow(i) * minCostFlow.UnitCost(i);
                        Console.WriteLine("Consultant" + minCostFlow.Tail(i) + " to Client" + minCostFlow.Head(i) + " with a traveltime of " + cost);
                    }
            }
            else
            {
                Console.WriteLine("Solving the min cost flow problem failed. Solver status: " +
                                  solveStatus);
            }
          

            stopWatch.Stop();
            //Wait and display solution
            Console.WriteLine("Minimum cost: " + optimalCost);
            //Console.WriteLine("The calculation took: {0} miliseconds", stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
            Console.ReadKey();
        }
    }
}
