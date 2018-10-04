from ortools.linear_solver import pywraplp

#Init the solver
solver = pywraplp.Solver('SolveStigler',
                           pywraplp.Solver.GLOP_LINEAR_PROGRAMMING)
 
#Model the table with consultants and their travel time to clients
travelTimes = [
[1, 1, 1 ],
[2, 2, 2 ],
[3, 3, 3]
];

cfg = { 'consultants':
        [['Consultant I', 1], ['Consultant II', 1], ['Consultant III', 1]],
        'clients':
        [['Client A', 1], ['Client B', 1], ['Client C', 1]]};

def configure_variables(cfg, solver):
  consultants = cfg['consultants']
  clients = cfg['clients']

  print('')
  print('creating variable_matrix')
  #variable_matrix will be a matrix of variables following the pattern [consultant_i][client_j]
  if (len(consultants)>len(clients)):
          variable_matrix = [[0 for i in range(len(consultants))] for j in range(len(clients))]
  else:
          variable_matrix = [[0 for i in range(len(clients))] for j in range(len(consultants))]

  variable_list = []
  for i in range(0, len(consultants)):
          for j in range(0, len(clients)):
                  variable_name = str('%s to %s' %(consultants[i][0],clients[j][0]))
                  print(variable_name + ' with travel time: ' + str(travelTimes[i][j]))
                  variable_list.append(solver.NumVar(0, 1, variable_name))
                  variable_matrix[i][j]=variable_list[-1]

  return variable_matrix

def configure_constraints(cfg, solver, variable_matrix):
  constraint_list=[]
  consultants = cfg['consultants']
  clients = cfg['clients']
  
  #one consultant can only go to one client
  print('--New Constraint, sum of Consultants to Clients = 1---')
  for i in range(0, len(consultants)):
    print('new constraint: 1 =')
    constraint_list.append(solver.Constraint(1,1))
    for j in range(0, len(clients)):
      print(1, variable_matrix[i][j], '+')
      constraint_list[-1].SetCoefficient(variable_matrix[i][j], 1)

  #one client can only get one consultant
  print('--New Constraint, sum of Clients at Consultants = 1---')
  for i in range(0, len(clients)):
    print('new constraint: 1 =')
    constraint_list.append(solver.Constraint(1,1))
    for j in range(0, len(consultants)):
      print(1, variable_matrix[j][i], '+')
      constraint_list[-1].SetCoefficient(variable_matrix[j][i], 1)

  return constraint_list     

def configure_objective(cfg, solver, var_matrix, constraint_list):
  consultants = cfg['consultants']
  clients = cfg['clients']

  objective = solver.Objective()

  print('minimize =')
  for i in range(0, len(consultants)):
    for j in range(0, len(clients)):
      print(travelTimes[i][j], var_matrix[i][j])
      objective.SetCoefficient(var_matrix[i][j], travelTimes[i][j])

  objective.SetMinimization()

  return objective

def print_solution(solver,result_status,variable_matrix,constraint_list):
  if result_status == solver.OPTIMAL:
    print('Successful solve.')
    # The problem has an optimal solution.
    print(('Problem solved in %f milliseconds' % solver.wall_time()))
    # The objective value of the solution.
    print(('Optimal objective value = %f' % solver.Objective().Value()))
    # The value of each variable in the solution.
    var_sum=0
    for row in variable_matrix:
            for cell in row:
                    print(('%s = %f' % (cell.name(), cell.solution_value())))
                    var_sum+=cell.solution_value()
    print(('Variable sum = %f' % var_sum));
    
  elif result_status == solver.INFEASIBLE:
          print('No solution found.')
  elif result_status == solver.POSSIBLE_OVERFLOW:
          print('Some inputs are too large and may cause an integer overflow.')

def solve(solver):
  try:
    result_status = solver.Solve()
  except Exception as e:
    print("type error: " + str(e))
  return result_status

def main(cfg):
  solver = pywraplp.Solver('SolveSimpleSystem',pywraplp.Solver.GLOP_LINEAR_PROGRAMMING)
  var_matrix = configure_variables(cfg, solver)
  constraint_list = configure_constraints(cfg, solver, var_matrix)
  objective = configure_objective(cfg, solver, var_matrix, constraint_list)
  result_status = solve(solver)
  print_solution(solver, result_status, var_matrix, constraint_list)

main(cfg)
