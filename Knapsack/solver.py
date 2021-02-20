#!/usr/bin/python
# -*- coding: utf-8 -*-

import os
from subprocess import Popen, PIPE


def solve_it(file_location):
    # Writes the inputData to a temporay file



    # Runs the command: java Solver -file=tmp.data
    # print file_location
    process = Popen(['dotnet', 'run', '--project', 'Discrete.Optimization.Knapsack.csproj', file_location], stdout=PIPE, universal_newlines=True)
    (stdout, stderr) = process.communicate()


    return stdout.strip()


import sys

if __name__ == '__main__':
    if len(sys.argv) > 1:
        file_location = sys.argv[1].strip()
        with open(file_location, 'r') as input_data_file:
            input_data = input_data_file.read()
        print(solve_it(file_location))
    else:
        print(
            'This test requires an input file.  Please select one from the data directory. (i.e. python solver.py ./data/ks_4_0)')
