using Google.OrTools.LinearSolver;
using Google.OrTools.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace standard_ORtools
{
    class Program
    {
        private static Service.MockDataGenerator mockDataGenerator;

        static void Main(string[] args)
        {
            /*
            Console.WriteLine("Running test program, press enter to continue.");
            Console.ReadLine();
            RunLinearProgrammingExample("GLOP_LINEAR_PROGRAMMING");
            */
            Console.WriteLine("Running test program, press enter to continue.");
            Console.ReadKey();
            testGenerator();
            //SolveMinCostFlow();
        }

        private static void testGenerator()
        {
            Console.WriteLine("Generating testdata.");
            mockDataGenerator = new Service.MockDataGenerator();
            var consultantList = mockDataGenerator.GenerateRandomConsultantList(500, 200);
            Console.WriteLine("Number of consultants: {0}, number of companies: {1}, id starts at: {2}",
                consultantList.Count(),
                consultantList[0].TraveltimesInMinutes.Count(),
                consultantList[0].ConsultantId);
            Console.ReadKey();
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

        private static void SolveMinCostFlow()
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
            Console.WriteLine("Trying to solve mincostflow");
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
    }
}
