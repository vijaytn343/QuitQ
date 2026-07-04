import { useEffect, useState } from "react";
import {
  getSalesReport,
  getSalesSummary
} from "../api/sellerApi";

import SellerLayout from "../layouts/SellerLayout";
import * as XLSX from "xlsx";
import { saveAs } from "file-saver";

import {
  ResponsiveContainer,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  PieChart,
  Pie,
  Cell
} from "recharts";

export default function SalesReport() {

  const [summary, setSummary] = useState(null);
  const [report, setReport] = useState([]);

  const COLORS = [
    "#2563EB",
    "#10B981",
    "#F59E0B",
    "#8B5CF6",
    "#EF4444",
    "#14B8A6",
    "#EC4899",
    "#F97316",
    "#06B6D4",
    "#84CC16"
  ];

  useEffect(() => {
    loadSummary();
    loadReport();
  }, []);

  const loadSummary = async () => {
    try {
      const response = await getSalesSummary();
      setSummary(response.data);
    } catch (error) {
      console.log(error);
    }
  };

  const loadReport = async () => {
    try {
      const response = await getSalesReport();

      const sorted = [...response.data].sort(
        (a, b) => b.revenue - a.revenue
      );

      setReport(sorted);

    } catch (error) {
      console.log(error);
    }
  };
  const exportExcel = () => {

  const excelData = report.map(item => ({
    Product: item.productName,
    "Quantity Sold": item.quantitySold,
    Revenue: item.revenue
  }));

  const worksheet =
    XLSX.utils.json_to_sheet(excelData);

  const workbook =
    XLSX.utils.book_new();

  XLSX.utils.book_append_sheet(
    workbook,
    worksheet,
    "Sales Report"
  );

  const excelBuffer =
    XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array"
    });

  const file =
    new Blob(
      [excelBuffer],
      {
        type:
          "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
      }
    );

  saveAs(file, "QuitQ_Sales_Report.xlsx");

};

  return (

    <SellerLayout>

      <h2 className="fw-bold mb-4">
        Sales Report
      </h2>

      {summary && (

        <div className="row g-4 mb-5">

          <div className="col-lg-3">

            <div className="card shadow-sm border-0 rounded-4 p-4 text-center">

              <h6 className="text-muted">
                Total Revenue
              </h6>

              <h2 className="fw-bold text-primary">
                ₹{summary.totalRevenue}
              </h2>

            </div>

          </div>

          <div className="col-lg-3">

            <div className="card shadow-sm border-0 rounded-4 p-4 text-center">

              <h6 className="text-muted">
                Weekly Revenue
              </h6>

              <h2 className="fw-bold text-success">
                ₹{summary.weeklyRevenue}
              </h2>

            </div>

          </div>

          <div className="col-lg-3">

            <div className="card shadow-sm border-0 rounded-4 p-4 text-center">

              <h6 className="text-muted">
                Monthly Revenue
              </h6>

              <h2 className="fw-bold text-warning">
                ₹{summary.monthlyRevenue}
              </h2>

            </div>

          </div>

          <div className="col-lg-3">

            <div className="card shadow-sm border-0 rounded-4 p-4 text-center">

              <h6 className="text-muted">
                Top Product
              </h6>

              <h5 className="fw-bold">
                {summary.monthlyTopProduct}
              </h5>

            </div>

          </div>

        </div>

      )}

      <div className="alert alert-primary rounded-4 mb-5">

        <h5 className="fw-bold">
          Business Insights
        </h5>

        <ul className="mb-0">

          <li>
            Total Products Sold :
            <strong> {summary?.productsSold}</strong>
          </li>

          <li>
            Weekly Revenue :
            <strong> ₹{summary?.weeklyRevenue}</strong>
          </li>

          <li>
            Total Revenue :
            <strong> ₹{summary?.totalRevenue}</strong>
          </li>

          <li>
            Top Product :
            <strong> {summary?.monthlyTopProduct}</strong>
          </li>

        </ul>

      </div>

      <div className="row g-4 mb-5">

        <div className="col-lg-8">

          <div className="card shadow-sm border-0 rounded-4 p-4">

            <h4 className="fw-bold mb-4">
              Revenue by Product
            </h4>

            <ResponsiveContainer
              width="100%"
              height={450}
            >

              <BarChart
                data={report}
                layout="vertical"
                margin={{
                  top: 20,
                  right: 30,
                  left: 90,
                  bottom: 20
                }}
              >

                <CartesianGrid strokeDasharray="3 3" />

                <XAxis type="number" />

                <YAxis
                  dataKey="productName"
                  type="category"
                  width={180}
                />

                <Tooltip />

                <Bar
                  dataKey="revenue"
                  fill="#2563EB"
                  radius={[0, 8, 8, 0]}
                />

              </BarChart>

            </ResponsiveContainer>

          </div>

        </div>

        <div className="col-lg-4">

          <div className="card shadow-sm border-0 rounded-4 p-4">

            <h4 className="fw-bold mb-4">
              Product Share
            </h4>

            <ResponsiveContainer
              width="100%"
              height={450}
            >

              <PieChart>

                <Pie
                  data={report}
                  dataKey="quantitySold"
                  nameKey="productName"
                  outerRadius={150}
                  label={false}
                >

                  {report.map((entry, index) => (

                    <Cell
                      key={index}
                      fill={
                        COLORS[
                          index % COLORS.length
                        ]
                      }
                    />

                  ))}

                </Pie>

                <Tooltip />

              </PieChart>

            </ResponsiveContainer>

          </div>

        </div>

      </div>

      {/* Product Sales Table */}
      <div className="card shadow-sm border-0 rounded-4">

  <div className="card-header bg-white d-flex justify-content-between align-items-center">

    <h4 className="fw-bold mb-0">
      Top Selling Products
    </h4>
<button
  className="btn btn-success"
  onClick={exportExcel}
>
  Export Excel
</button>
  </div>

  <div className="card-body">

    <table className="table table-hover align-middle">

      <thead className="table-light">

        <tr>

          <th>#</th>

          <th>Product</th>

          <th>Quantity Sold</th>

          <th>Revenue</th>

          <th>Contribution</th>

        </tr>

      </thead>

      <tbody>

        {report.length > 0 ? (

          report.map((item, index) => {

            const percentage =
              summary?.totalRevenue > 0
                ? (
                    (item.revenue /
                      summary.totalRevenue) *
                    100
                  ).toFixed(1)
                : 0;

            return (

              <tr key={index}>

                <td>
                  {index + 1}
                </td>

                <td className="fw-semibold">
                  {item.productName}
                </td>

                <td>
                  {item.quantitySold}
                </td>

                <td className="fw-bold text-success">
                  ₹{item.revenue}
                </td>

                <td style={{ width: "220px" }}>

                  <div className="progress">

                    <div
                      className="progress-bar"
                      role="progressbar"
                      style={{
                        width: `${percentage}%`
                      }}
                    >
                      {percentage}%
                    </div>

                  </div>

                </td>

              </tr>

            );

          })

        ) : (

          <tr>

            <td
              colSpan="5"
              className="text-center py-5"
            >
              No Sales Found
            </td>

          </tr>

        )}

      </tbody>

    </table>

  </div>

</div>

<div className="row mt-4">

  <div className="col-md-4">

    <div className="card border-0 shadow-sm rounded-4 p-4">

      <h5 className="fw-bold">
        Total Products
      </h5>

      <h2 className="text-primary">
        {report.length}
      </h2>

    </div>

  </div>

  <div className="col-md-4">

    <div className="card border-0 shadow-sm rounded-4 p-4">

      <h5 className="fw-bold">
        Products Sold
      </h5>

      <h2 className="text-success">
        {summary?.productsSold}
      </h2>

    </div>

  </div>

  <div className="col-md-4">

    <div className="card border-0 shadow-sm rounded-4 p-4">

      <h5 className="fw-bold">
        Avg Revenue / Product
      </h5>

      <h2 className="text-warning">

        ₹

        {report.length > 0

          ? (
              summary?.totalRevenue /
              report.length
            ).toFixed(0)

          : 0}

      </h2>

    </div>

  </div>

</div>

</SellerLayout>

  );

}