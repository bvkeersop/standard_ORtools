using Google.OrTools.LinearSolver;
using standard_ORtools.Model;
using System;
using System.Collections.Generic;

namespace standard_ORtools.ModelSolvers
{
    class CDLinearSolver
    {
        private Solver solver;

        public CDLinearSolver(List<Consultant> consultants)
        {
            solver = Solver.CreateSolver("CDLinearSolver", "GLOP_LINEAR_PROGRAMMING");
            var variableMatrix = GenerateVariableMatrix(consultants);
            PrintMatrix(variableMatrix);
            ConfigureConstraints(consultants.Count, consultants[0].ClientAndTravelTime.Count, variableMatrix);
            setObjective();
            var resultStatus = Solve();
            PrintSolution(resultStatus, variableMatrix);
            Console.ReadKey();
        }

        private List<Variable[]> GenerateVariableMatrix(List<Consultant> consultants)
        {
            var x = consultants[1].ClientAndTravelTime[1];
            List<Variable[]> variableMatrix = new List<Variable[]>();
            int ammountofClients = consultants[0].ClientAndTravelTime.Count;

            var variableList = new List<Variable>();

            for(int i = 0; i < consultants.Count; i++)
            {
                Console.WriteLine(i);
                Variable[] row = new Variable[ammountofClients];

                for(int j =0; j < consultants[i].ClientAndTravelTime.Count; j++)
                {
                    string variableName = string.Format("Consultant{0}ToClient{1}", 
                        consultants[i].ConsultantId,
                        consultants[i].ClientAndTravelTime[j].ClientId); 
                    var variable = solver.MakeIntVar(0, 1, variableName);
                    variableList.Add(variable);
                    row[j] = variable;
                }
                variableMatrix.Add(row);
            }
            return variableMatrix;
        }

        private List<Constraint> ConfigureConstraints(int numberOfConsultants, int numberOfClients, List<Variable[]> variableMatrix, List<Consultant> consultants)
        {
            var constraintList = new List<Constraint>();

            //Set the constraint so that the sum of the consultant 1 <= Demand client 1...100
            for(int i = 0; i < numberOfConsultants; i++)
            {
                var constraint = solver.MakeConstraint(1, 1); //Supply
                for(int j = 0; j < numberOfClients; j++)
                {
                    constraint.SetCoefficient(variableMatrix[i][j], 1);
                }
                constraintList.Add(constraint);
            }

            //Set the constraint so that the sum of the consultant 1..100 <= Demand client 1
            for (int i = 0; i < numberOfConsultants; i++)
            {
                var constraint = solver.MakeConstraint(1, 1); //Demand
                for (int j = 0; j < numberOfClients; j++)
                {
                    constraint.SetCoefficient(variableMatrix[j][i], 1);
                }
                constraintList.Add(constraint);
            }

            return constraintList;
        }

        private Objective setObjective(int numberOfConsultants, int numberOfClients, List<Variable[]> variableMatrix)
        {
            Objective objective = solver.Objective();

            for (int i = 0; i < numberOfConsultants; i++)
            {
                for (int j = 0; j < numberOfClients; j++)
                {
                    objective.SetCoefficient(variableMatrix[i][j]);
                    consultants[i].ClientAndTravelTime[j].TimeInMinutes
                }
            }

            consultants[i].ClientAndTravelTime[j].TimeInMinutes
            objective.SetMinimization();
            return objective;
        }

        private int Solve()
        {
            var resultStatus = solver.Solve();
            return resultStatus;
        }

        private void PrintSolution(int resultStatus, List<Variable[]> variableMatrix)
        {
            if (resultStatus == Solver.OPTIMAL)
            {
                Console.WriteLine("Problem solved in " + solver.WallTime() +
                     " milliseconds");

                // The objective value of the solution.
                Console.WriteLine("Optimal objective value = " +
                                  solver.Objective().Value());

                // The value of each variable in the solution.
                foreach(var row in variableMatrix)
                {
                    for(int i = 0; i < row.Length; i++)
                    {
                        double solutionValue = row[i].SolutionValue();
                        if(solutionValue > 0)
                        {

                            Console.WriteLine(row[i].Name() + " for an ammount of " + row[i].SolutionValue());
                        }
                    }
                }
            

                Console.WriteLine("Advanced usage:");
                Console.WriteLine("Problem solved in " + solver.Nodes() +
                                  " branch-and-bound nodes");
            }
            else
            {
                Console.WriteLine("ERROR");
            }
        }

        public void PrintMatrix(List<Variable[]> variableMatrix)
        {
            foreach(var x in variableMatrix)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    Console.WriteLine(x[i].Name());
                }
            }
        }
    }
}
