import React, { useEffect, useState } from "react";
import { Bar, Line } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  LineElement,
  PointElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
import { moneyStatistics, productStatistics } from "@queries/AdminAPI";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  LineElement,
  PointElement,
  Title,
  Tooltip,
  Legend
);
interface DataChart {
  labels: string[]; // Array of strings for labels
  datasets: {
    label: string;
    data: number[]; // Array of numbers for data points
    backgroundColor: string;
  }[];
}

interface DataMoneyChart {
  labels: string[]; // Array of strings for labels
  datasets: {
    label: string;
    data: number[]; // Array of numbers for data points
    fill: boolean;
    borderColor: string,
    backgroundColor: string,
  }[];
}

const Home = () => {
  const [dailyData, setDailyData] = useState<DataChart>({
    labels: [],
    datasets: [{ label: "", data: [], backgroundColor: "" }],
  });
  const [monthlyData, setMonthlyData] = useState<DataMoneyChart>({
    labels: [],
    datasets: [{ label: "", data: [], fill: false, borderColor: "", backgroundColor: "" }],
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await productStatistics();
        const responsemoney = await moneyStatistics();
        setMonthlyData(responsemoney);
        setDailyData(response);
      } catch (error) {}
    };
    fetchData();
    console.log("monthlyData", monthlyData);
    
  }, []);

  return (
    <div className="container mx-auto p-6 bg-gray-100 min-h-screen mt-16">
      <div className="grid grid-cols-4 gap-4 mb-6">
        <div className="bg-white p-4 rounded-lg shadow-sm flex items-center justify-between">
          <svg className="w-6 h-6 text-gray-800 dark:text-white mr-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 24 24">
            <path fill-rule="evenodd" d="M7 6a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2h-2v-4a3 3 0 0 0-3-3H7V6Z" clip-rule="evenodd"/>
            <path fill-rule="evenodd" d="M2 11a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-7Zm7.5 1a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5Z" clip-rule="evenodd"/>
            <path d="M10.5 14.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z"/>
          </svg>
          <div>
            <div className="text-3xl font-bold text-gray-800"> 
              5.000.000 VNĐ
            </div>
            <div className="text-sm text-gray-500">Today's Money</div>
          </div>
        </div>

        
        <div className="bg-white p-4 rounded-lg shadow-sm flex items-center justify-between">
          <svg className="w-6 h-6 text-gray-800 dark:text-white mr-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 24 24">
            <path fill-rule="evenodd" d="M7 6a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2h-2v-4a3 3 0 0 0-3-3H7V6Z" clip-rule="evenodd"/>
            <path fill-rule="evenodd" d="M2 11a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-7Zm7.5 1a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5Z" clip-rule="evenodd"/>
            <path d="M10.5 14.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z"/>
          </svg>
          <div>
            <div className="text-3xl font-bold text-gray-800"> 
              5.000.000 VNĐ
            </div>
            <div className="text-sm text-gray-500">Today's Money</div>
          </div>
        </div>
        
        <div className="bg-white p-4 rounded-lg shadow-sm flex items-center justify-between">
          <svg className="w-6 h-6 text-gray-800 dark:text-white mr-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 24 24">
            <path fill-rule="evenodd" d="M7 6a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2h-2v-4a3 3 0 0 0-3-3H7V6Z" clip-rule="evenodd"/>
            <path fill-rule="evenodd" d="M2 11a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-7Zm7.5 1a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5Z" clip-rule="evenodd"/>
            <path d="M10.5 14.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z"/>
          </svg>
          <div>
            <div className="text-3xl font-bold text-gray-800"> 
              5.000.000 VNĐ
            </div>
            <div className="text-sm text-gray-500">Today's Money</div>
          </div>
        </div>
        
        <div className="bg-white p-4 rounded-lg shadow-sm flex items-center justify-between">
          <svg className="w-6 h-6 text-gray-800 dark:text-white mr-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 24 24">
            <path fill-rule="evenodd" d="M7 6a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2h-2v-4a3 3 0 0 0-3-3H7V6Z" clip-rule="evenodd"/>
            <path fill-rule="evenodd" d="M2 11a2 2 0 0 1 2-2h11a2 2 0 0 1 2 2v7a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2v-7Zm7.5 1a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5Z" clip-rule="evenodd"/>
            <path d="M10.5 14.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0Z"/>
          </svg>
          <div>
            <div className="text-3xl font-bold text-gray-800"> 
              5.000.000 VNĐ
            </div>
            <div className="text-sm text-gray-500">Today's Money</div>
          </div>
        </div>
      </div>

      {/* Charts Section */}
      <div className="grid grid-cols-2 gap-4 mb-6">
        {/* Biểu đồ tiền theo ngày */}
        <div className="bg-white p-4 rounded-lg shadow-sm">
          <h2 className="text-lg font-semibold">Daily Money</h2>
          <Bar data={dailyData} options={{ responsive: true, plugins: { legend: { display: true } } }} />
          <div className="text-xs text-gray-500 mt-2">Updated daily</div>
        </div>

        {/* Biểu đồ tiền theo tháng */}
        <div className="bg-white p-4 rounded-lg shadow-sm">
          <h2 className="text-lg font-semibold">Monthly Money</h2>
          <Line data={monthlyData} options={{ responsive: true, plugins: { legend: { display: true } } }} />
          <div className="text-xs text-gray-500 mt-2">Updated monthly</div>
        </div>
      </div>

      {/* Projects Table */}
      <div className="bg-white p-4 rounded-lg shadow-sm">
        <div className="text-lg font-semibold mb-2">Projects</div>
        <div className="text-xs text-gray-500 mb-4">30 done this month</div>
        <div className="overflow-x-auto">
          <table className="min-w-full">
            <thead>
              <tr>
                <th className="px-4 py-2 text-left text-xs font-medium text-gray-500">Companies</th>
                <th className="px-4 py-2 text-left text-xs font-medium text-gray-500">Members</th>
                <th className="px-4 py-2 text-left text-xs font-medium text-gray-500">Budget</th>
                <th className="px-4 py-2 text-left text-xs font-medium text-gray-500">Completion</th>
              </tr>
            </thead>
            <tbody className="text-sm">
              <tr className="border-t">
                <td className="px-4 py-2">Material XD Version</td>
                <td className="px-4 py-2">Icons</td>
                <td className="px-4 py-2">$14,000</td>
                <td className="px-4 py-2">
                  <div className="w-full bg-gray-200 rounded">
                    <div className="bg-blue-500 text-xs font-medium text-blue-100 text-center p-0.5 leading-none rounded-l" style={{ width: "60%" }}>60%</div>
                  </div>
                </td>
              </tr>
              {/* Additional rows can be added here */}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Home;
