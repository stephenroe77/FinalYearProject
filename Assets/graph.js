import React, { useState } from "react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

const TrainingVisualization = () => {
  // Parse the CSV data
  const rawData = `Episode,Successes,Collisions,TotalSteps,AverageReward,CurriculumStage
100,96,4,23565,-12.92398,0
100,98,2,23859,-15.0005,0
100,99,1,24197,-15.46499,0
100,97,3,24386,-16.45462,0
100,98,2,24517,-17.5038,0
100,98,2,25320,-16.12617,0
100,98,2,25349,-17.94605,0
100,99,1,25531,-17.0723,0
100,98,2,25919,-18.40096,0
100,99,1,26384,-17.72654,0
100,97,3,27058,-25.93906,0
100,98,2,27142,-20.46684,0
100,98,2,27342,-20.21256,0
100,98,2,28176,-21.89147,0
100,97,3,28306,-20.23862,0
100,97,3,28475,-27.98297,0
200,194,6,48343,-14.47393,0
200,196,4,49768,-15.66792,0
200,196,4,50268,-16.74463,0
200,197,3,50450,-17.27545,0
200,195,5,50911,-19.25306,0
200,197,3,51011,-16.45766,0
200,197,3,51360,-18.62958,0
200,195,5,51646,-16.7209,0
200,196,4,52601,-19.185,0
200,196,4,52830,-21.25385,0
200,194,6,52987,-21.58744,0
200,197,3,53061,-21.60351,0
200,195,5,53513,-21.53935,0
200,194,6,53624,-25.03001,0
200,194,6,53676,-19.99574,0
200,196,4,55647,-21.28075,0
300,293,7,74941,-15.92167,0
300,296,4,75064,-16.19847,0
300,295,5,75271,-16.9383,0
300,297,3,75533,-17.29642,0
300,293,7,76486,-16.53714,0
300,294,6,76933,-18.71382,0
300,294,6,77439,-18.72155,0
300,295,5,77784,-21.00263,0
300,296,4,78089,-18.37266,0
300,291,9,78431,-22.90939,0
300,292,8,78706,-20.20467,0
300,290,10,79132,-20.04065,0
300,293,7,79914,-20.33181,0
300,293,7,79921,-20.90752,0
300,296,4,80757,-19.35761,0
300,292,8,80810,-19.378,0
400,396,4,98314,-16.16643,0
400,394,6,98748,-15.8413,0
400,393,7,99756,-15.79439,0
400,393,7,99924,-15.83053,0
400,391,9,101090,-16.40098,0
400,392,8,101463,-17.87394,0
400,392,8,102148,-17.22162,0
400,393,7,102291,-19.55131,0
400,389,11,104509,-19.53304,0
400,387,13,104803,-19.34001,0
400,391,9,105140,-19.78029,0
400,387,13,105253,-23.04253,0
400,387,13,105915,-17.91372,0
400,395,5,107552,-19.43034,0
400,388,12,108139,-21.66087,0
400,389,11,109904,-22.97596,0
500,491,9,123432,-15.7937,0
500,489,11,124867,-16.93459,0
500,494,6,125347,-16.92167,0
500,489,11,125749,-16.32727,0
500,490,10,126235,-16.88714,0
500,493,7,126280,-18.9095,0
500,491,9,127127,-17.07375,0
500,492,8,127442,-17.36265,0
500,484,16,129336,-18.41428,0
500,490,10,129462,-19.05166,0
500,486,14,131447,-19.66227,0
500,484,16,132074,-19.04848,0
500,495,5,132421,-18.47043,0
500,484,16,132569,-22.33629,0
500,487,13,132885,-19.49142,0
500,488,12,134242,-21.70062,0
600,588,12,145841,-15.19609,0
600,593,7,149308,-16.42201,0
600,591,9,151123,-18.38857,0
600,585,15,152123,-16.61619,0
600,588,12,152778,-17.76664,0
600,586,14,152830,-18.28029,0
600,587,13,153266,-18.31256,0
600,589,11,154857,-17.50793,0
600,591,9,155083,-17.53405,0
600,582,18,155742,-18.57372,0
600,581,19,157882,-21.5306,0
600,582,18,157962,-19.89592,0
600,591,9,158348,-18.89051,0
600,582,18,158747,-19.16935,0
600,586,14,159996,-19.4739,0
600,588,12,160481,-21.10464,0
700,687,13,170964,-15.43611,0
700,690,10,173970,-16.43181,0
700,690,10,175400,-17.93635,0
700,684,16,177513,-16.79434,0
700,683,17,178996,-18.43826,0
700,684,16,179834,-18.58775,0
700,686,14,180558,-17.3611,0
700,679,21,181019,-18.27171,0
700,689,11,181472,-17.7893,0
700,686,14,181576,-18.55002,0
700,689,11,181733,-18.31057,0
700,679,21,182808,-19.35831,0
700,679,21,184054,-21.03833,0
700,681,19,184677,-19.139,0
700,685,15,186597,-19.69752,0
700,685,15,187818,-21.33995,0
800,784,16,199590,-16.66471,0
800,788,12,202043,-18.25051,0
800,790,10,202289,-17.09676,0
800,780,20,203160,-17.06101,0
800,783,17,204653,-18.00501,0
800,780,20,205324,-18.4821,0
800,781,19,205408,-18.47268,0
800,769,31,205835,-17.96964,0
800,782,18,206211,-17.37588,0
800,778,22,208222,-18.93499,0
800,778,22,209621,-20.41188,0
800,785,15,209650,-18.4322,0
800,786,14,210058,-19.14768,0
800,779,21,211171,-19.1934,0
800,785,15,213208,-19.62367,0
800,785,15,214001,-20.9148,0
900,879,21,227439,-17.79257,0
900,887,13,227796,-17.07908,0
900,881,19,228607,-17.34872,0
900,885,15,229144,-18.8965,0
900,876,24,229925,-17.58857,0
900,874,26,230061,-18.32068,0
900,867,33,231607,-17.89691,0
900,879,21,231874,-19.43442,0
900,874,26,232153,-18.54324,0
900,880,20,233550,-17.56696,0
900,884,16,234551,-18.32704,0
900,875,25,235730,-20.53514,0
900,884,16,236619,-19.26639,0
900,879,21,236784,-19.036,0
900,884,16,240078,-19.36166,0
900,885,15,240115,-20.71336,0
1000,978,22,251758,-17.61897,0
1000,979,21,252899,-17.168,0
1000,981,19,253660,-17.10705,0
1000,975,25,255682,-17.69159,0
1000,974,26,255874,-18.88494,0
1000,985,15,256889,-19.06649,0
1000,980,20,257170,-17.51097,0
1000,966,34,257556,-17.82891,0
1000,970,30,258906,-18.51024,0
1000,972,28,259287,-18.76086,0
1000,972,28,260898,-20.35666,0
1000,978,22,261428,-18.69119,0
1000,982,18,262603,-19.43662,0
1000,983,17,262940,-18.75109,0
1000,983,17,267181,-20.41283,0
1000,983,17,268088,-20.95986,0
1100,1078,22,276871,-16.83972,0
1100,1077,23,278571,-17.83599,0
1100,1071,29,279233,-17.4281,0
1100,1079,21,280241,-17.15381,0
1100,1079,21,280539,-17.45848,0
1100,1085,15,280620,-18.55132,0
1100,1072,28,281262,-18.76121,0
1100,1065,35,282390,-17.59566,0
1100,1072,28,284578,-18.49816,0
1100,1069,31,284873,-18.92234,0
1100,1080,20,286689,-19.03739,0
1100,1068,32,287424,-20.21415,0
1100,1081,19,288860,-18.60736,0
1086,1067,18,291651,-20.51349,0
1126,1097,28,291651,-18.54782,0
1146,1129,16,291651,-18.40493,0
1089,1070,18,291651,-20.7603,0
1141,1119,21,291651,-17.48685,0
1121,1099,21,291651,-18.98013,0
1100,1077,22,291651,-19.71591,0
1114,1081,32,291651,-20.23612,0
1112,1091,20,291651,-18.54936,0
1149,1124,24,291651,-17.23422,0
1151,1121,29,291651,-17.33125,0
1140,1116,23,291651,-17.56547,0
1131,1095,35,291651,-17.83284,0
1142,1113,28,291651,-18.87432,0
1128,1095,32,291651,-18.85878,0
1151,1125,25,291651,-17.81376,0`;

  // Process the data
  const rows = rawData.split("\n");
  const headers = rows[0].split(",");
  const parsedData = [];

  // Group data by episode milestone (100, 200, etc.)
  const episodeMilestones = {};

  // Parse all rows except header
  for (let i = 1; i < rows.length; i++) {
    const values = rows[i].split(",");
    if (values.length === headers.length) {
      const entry = {};
      for (let j = 0; j < headers.length; j++) {
        // Convert numerical values to numbers
        if (j > 0) {
          entry[headers[j]] = parseFloat(values[j]);
        } else {
          entry[headers[j]] = values[j];
        }
      }

      // Group data points by episode milestone
      const episodeMilestone = Math.floor(parseInt(entry.Episode) / 100) * 100;
      if (!episodeMilestones[episodeMilestone]) {
        episodeMilestones[episodeMilestone] = [];
      }
      episodeMilestones[episodeMilestone].push(entry);

      parsedData.push(entry);
    }
  }

  // Calculate aggregate metrics for each episode milestone
  const aggregateData = Object.keys(episodeMilestones).map((milestone) => {
    const entries = episodeMilestones[milestone];

    // Calculate averages for each metric
    const aggregate = {
      Episode: parseInt(milestone),
      AvgSuccesses:
        entries.reduce((sum, entry) => sum + entry.Successes, 0) /
        entries.length,
      AvgCollisions:
        entries.reduce((sum, entry) => sum + entry.Collisions, 0) /
        entries.length,
      AvgSteps:
        entries.reduce((sum, entry) => sum + entry.TotalSteps, 0) /
        entries.length,
      AvgReward:
        entries.reduce((sum, entry) => sum + entry.AverageReward, 0) /
        entries.length,
      SuccessRate:
        (entries.reduce((sum, entry) => sum + entry.Successes, 0) /
          entries.reduce((sum, entry) => sum + parseInt(entry.Episode), 0)) *
        100,
    };

    return aggregate;
  });

  // Sort the aggregated data by episode
  aggregateData.sort((a, b) => a.Episode - b.Episode);

  // Calculate success rate
  const successRateData = parsedData.map((entry) => ({
    Episode: entry.Episode,
    SuccessRate: (entry.Successes / parseInt(entry.Episode)) * 100,
  }));

  // Metrics to display
  const metrics = [
    { key: "AvgSuccesses", name: "Average Successes", color: "#8884d8" },
    { key: "AvgCollisions", name: "Average Collisions", color: "#d88884" },
    { key: "AvgReward", name: "Average Reward", color: "#82ca9d" },
    { key: "SuccessRate", name: "Success Rate (%)", color: "#ffc658" },
  ];

  const [selectedMetric, setSelectedMetric] = useState(metrics[0]);

  return (
    <div className="flex flex-col space-y-6 w-full p-4">
      <h1 className="text-2xl font-bold text-center">
        Reinforcement Learning Training Results
      </h1>

      <div className="flex flex-col space-y-2">
        <h2 className="text-xl font-semibold">Select Metric to Display:</h2>
        <div className="flex flex-wrap gap-2">
          {metrics.map((metric) => (
            <button
              key={metric.key}
              className={`px-3 py-1 rounded border ${
                selectedMetric.key === metric.key
                  ? "bg-blue-500 text-white border-blue-600"
                  : "bg-white text-gray-800 border-gray-300 hover:bg-gray-100"
              }`}
              onClick={() => setSelectedMetric(metric)}
            >
              {metric.name}
            </button>
          ))}
        </div>
      </div>

      <div className="h-96 w-full">
        <ResponsiveContainer width="100%" height="100%">
          <LineChart
            data={aggregateData}
            margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis
              dataKey="Episode"
              label={{
                value: "Episode",
                position: "insideBottomRight",
                offset: -5,
              }}
            />
            <YAxis
              label={{
                value: selectedMetric.name,
                angle: -90,
                position: "insideLeft",
                style: { textAnchor: "middle" },
              }}
            />
            <Tooltip formatter={(value) => value.toFixed(2)} />
            <Legend />
            <Line
              type="monotone"
              dataKey={selectedMetric.key}
              name={selectedMetric.name}
              stroke={selectedMetric.color}
              activeDot={{ r: 8 }}
              strokeWidth={2}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>

      <div className="flex flex-col space-y-4">
        <h2 className="text-xl font-semibold">Training Progress Summary</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <div className="p-4 bg-blue-50 rounded-lg border border-blue-200">
            <h3 className="text-lg font-medium text-blue-800">
              Final Success Rate
            </h3>
            <p className="text-2xl font-bold">
              {aggregateData[aggregateData.length - 1].SuccessRate.toFixed(2)}%
            </p>
          </div>
          <div className="p-4 bg-green-50 rounded-lg border border-green-200">
            <h3 className="text-lg font-medium text-green-800">
              Avg Reward (Last Batch)
            </h3>
            <p className="text-2xl font-bold">
              {aggregateData[aggregateData.length - 1].AvgReward.toFixed(2)}
            </p>
          </div>
          <div className="p-4 bg-yellow-50 rounded-lg border border-yellow-200">
            <h3 className="text-lg font-medium text-yellow-800">
              Total Episodes
            </h3>
            <p className="text-2xl font-bold">
              {parsedData[parsedData.length - 1].Episode}
            </p>
          </div>
          <div className="p-4 bg-purple-50 rounded-lg border border-purple-200">
            <h3 className="text-lg font-medium text-purple-800">
              Avg Steps per Episode
            </h3>
            <p className="text-2xl font-bold">
              {(
                aggregateData[aggregateData.length - 1].AvgSteps /
                aggregateData[aggregateData.length - 1].Episode
              ).toFixed(2)}
            </p>
          </div>
        </div>
      </div>

      <div className="h-96 w-full">
        <h2 className="text-xl font-semibold mb-4">
          Success vs Collision Trend
        </h2>
        <ResponsiveContainer width="100%" height="100%">
          <LineChart
            data={aggregateData}
            margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
          >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis
              dataKey="Episode"
              label={{
                value: "Episode",
                position: "insideBottomRight",
                offset: -5,
              }}
            />
            <YAxis
              label={{
                value: "Count",
                angle: -90,
                position: "insideLeft",
                style: { textAnchor: "middle" },
              }}
            />
            <Tooltip formatter={(value) => value.toFixed(2)} />
            <Legend />
            <Line
              type="monotone"
              dataKey="AvgSuccesses"
              name="Average Successes"
              stroke="#8884d8"
              strokeWidth={2}
            />
            <Line
              type="monotone"
              dataKey="AvgCollisions"
              name="Average Collisions"
              stroke="#d88884"
              strokeWidth={2}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default TrainingVisualization;
