import { useEffect, useState } from "react";
import {
  getSellerOrders,
  updateOrderStatus
} from "../api/sellerOrderApi";
import { toast } from "react-toastify";
import SellerLayout from "../layouts/SellerLayout";

export default function SellerOrders() {
  const [orders, setOrders] = useState([]);

  useEffect(() => {
    loadOrders();
  }, []);

  const loadOrders = async () => {
    try {
      const response = await getSellerOrders();
      setOrders(response.data);
    } catch (error) {
      console.log(error);
    }
  };

  const changeStatus = async (orderId, status) => {
    try {
      await updateOrderStatus(orderId, status);

      toast.success("Order Status Updated Successfully");

      loadOrders();
    } catch (error) {
      console.log(error);

      toast.error(
        error.response?.data?.message ||
          "Failed to update order status"
      );
    }
  };

  return (
    <SellerLayout>

      <h2 className="fw-bold mb-4">
        Seller Orders
      </h2>

      {/* Summary Cards */}

      <div className="row g-4 mb-4">

        <div className="col-md-4">
          <div className="card border-0 shadow-sm rounded-4 p-4 text-center">
            <h6 className="text-muted">
              Total Orders
            </h6>

            <h2 className="fw-bold">
              {orders.length}
            </h2>
          </div>
        </div>

        <div className="col-md-4">
          <div className="card border-0 shadow-sm rounded-4 p-4 text-center">
            <h6 className="text-muted">
              Pending
            </h6>

            <h2 className="fw-bold text-warning">
              {
                orders.filter(
                  o => o.orderStatus === "Pending"
                ).length
              }
            </h2>
          </div>
        </div>

        <div className="col-md-4">
          <div className="card border-0 shadow-sm rounded-4 p-4 text-center">
            <h6 className="text-muted">
              Delivered
            </h6>

            <h2 className="fw-bold text-success">
              {
                orders.filter(
                  o => o.orderStatus === "Delivered"
                ).length
              }
            </h2>
          </div>
        </div>

      </div>

      {/* Orders */}

      {orders.map((order) => (

        <div
          key={`${order.orderId}-${order.productName}`}
          className="card border-0 shadow-sm rounded-4 p-4 mb-4"
        >

          <div className="d-flex justify-content-between align-items-center">

            <h5 className="fw-bold">
              Order #{order.orderId}
            </h5>

            {order.orderStatus === "Pending" && (
              <span className="badge bg-warning text-dark">
                Pending
              </span>
            )}

            {order.orderStatus === "Shipped" && (
              <span className="badge bg-primary">
                Shipped
              </span>
            )}

            {order.orderStatus === "Delivered" && (
              <span className="badge bg-success">
                Delivered
              </span>
            )}

          </div>

          <hr />

          <div className="row">

            <div className="col-md-6">

              <p>
                <strong>Customer:</strong>
                {" "}
                {order.customerName}
              </p>

              <p>
                <strong>Product:</strong>
                {" "}
                {order.productName}
              </p>

            </div>

            <div className="col-md-6">

              <p>
                <strong>Quantity:</strong>
                {" "}
                {order.quantity}
              </p>

              <p>
                <strong>Price:</strong>
                {" "}
                ₹{order.priceAtPurchase}
              </p>

            </div>

          </div>

          <div className="mt-3">

            {order.orderStatus === "Pending" && (

              <button
                className="btn btn-primary"
                onClick={() =>
                  changeStatus(
                    order.orderId,
                    "Shipped"
                  )
                }
              >
                Mark as Shipped
              </button>

            )}

            {order.orderStatus === "Shipped" && (

              <button
                className="btn btn-success"
                onClick={() =>
                  changeStatus(
                    order.orderId,
                    "Delivered"
                  )
                }
              >
                Mark as Delivered
              </button>

            )}

            {order.orderStatus === "Delivered" && (

              <button
                className="btn btn-success"
                disabled
              >
                Delivered
              </button>

            )}

          </div>

        </div>

      ))}

    </SellerLayout>
  );
}