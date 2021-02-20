import java.io.*;
import java.util.List;
import java.util.ArrayList;
import java.util.Arrays;

/**
 * The class <code>Solver</code> is an implementation of a greedy algorithm to solve the knapsack problem.
 */

public class NumberWithIndex {
    public int Value;
    public int Index;

    public NumberWithIndex(int value, int index) {
        Value = value;
        Index = index;
    }
}

public class Solver {

    /**
     * The main class
     */
    public static void main(String[] args) {
        try {
            solve(args);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    /**
     * Read the instance, solve it, and print the solution in the standard output
     */
    public static void solve(String[] args) throws IOException {
        String fileName = null;

        // get the temp file name
        for (String arg : args) {
            if (arg.startsWith("-file=")) {
                fileName = arg.substring(6);
            }
        }
        if (fileName == null)
            return;

        // read the lines out of the file
        List<String> lines = new ArrayList<String>();

        BufferedReader input = new BufferedReader(new FileReader(fileName));
        try {
            String line = null;
            while ((line = input.readLine()) != null) {
                lines.add(line);
            }
        } finally {
            input.close();
        }
//                  SORTING rule
//        users.sort(Comparator.comparing(User::getCreatedOn).reversed());

        // parse the data in the file
        String[] firstLine = lines.get(0).split("\\s+");
        int items = Integer.parseInt(firstLine[0]);
        int capacity = Integer.parseInt(firstLine[1]);

        NumberWithIndex[] values = new NumberWithIndex[items];
        int[] valuesArray = new int[items];
        NumberWithIndex[] weights = new NumberWithIndex[items];
        int[] weightsArray = new int[items];

        for (int i = 1; i < items + 1; i++) {
            String line = lines.get(i);
            String[] parts = line.split("\\s+");

            int value = Integer.parseInt(parts[0]);
            int weight = Integer.parseInt(parts[1]);
            valuesArray[i - 1] = value;
            values[i - 1] = new NumberWithIndex(value, i - 1);
            weightsArray[i - 1] = weight;
            weights[i - 1] = new NumberWithIndex(weight, i - 1);
        }

        // a trivial greedy algorithm for filling the knapsack
        // it takes items in-order until the knapsack is full
        int value = 0;
        int weight = 0;
        int[] taken = new int[items];

        // sort by weight
        Arrays.sort(weights, Comparator.comparing(NumberWithIndex::Value).reversed());

        for (int i = 0; i < items; i++) {
            if (weight + weights[i] <= capacity) {
                taken[i] = 1;
                value += values[i];
                weight += weights[i];
            } else {
                taken[i] = 0;
            }
        }

        // prepare the solution in the specified output format
        System.out.println(value + " 0");
        for (int i = 0; i < items; i++) {
            System.out.print(taken[i] + " ");
        }
        System.out.println("");
    }
}