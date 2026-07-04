import { useEffect, useState } from "react";
import { getMyOrders } from "../api/orderApi";
import { downloadInvoice } from "../api/orderApi";
import { toast } from "react-toastify";
export default function MyOrders() {

  const [orders, setOrders] = useState([]);

  useEffect(() => {
    loadOrders();
  }, []);

  const loadOrders = async () => {
    try {
      const response = await getMyOrders();
      setOrders(response.data);
    } catch (error) {
      console.log(error);

  toast.error(
    error.response?.data?.message || "Failed to load orders"
  );
    }
  };
const handleDownloadInvoice = async (
  orderId
) => {
  try {

    const response =
      await downloadInvoice(orderId);

    const file = new Blob(
      [response.data],
      { type: "application/pdf" }
    );

    const fileURL =
      window.URL.createObjectURL(file);

    window.open(fileURL);

  } catch (error) {
    console.log(error);
    toast.error(
  error.response?.data?.message || "Failed to download invoice"
);
  }
};

  return (
    <div className="container mt-4">
      <div className="row mb-4">

  <div className="col-md-4">
    <div className="card text-center p-3 shadow-sm">
      <h5>Total Orders</h5>
      <h3>{orders.length}</h3>
    </div>
  </div>

  <div className="col-md-4">
    <div className="card text-center p-3 shadow-sm">
      <h5>Pending</h5>
      <h3>
        {
          orders.filter(
            o => o.orderStatus === "Pending"
          ).length
        }
      </h3>
    </div>
  </div>

  <div className="col-md-4">
    <div className="card text-center p-3 shadow-sm">
      <h5>Delivered</h5>
      <h3>
        {
          orders.filter(
            o => o.orderStatus === "Delivered"
          ).length
        }
      </h3>
    </div>
  </div>

</div>
      <h2>My Orders</h2>

      {orders.length === 0 ? (
        <p>No Orders Found</p>
      ) : (
        orders.map(order => (
          <div
            key={order.orderId}
           className="card p-4 mb-4 shadow-sm border-0"
          >
            <h5>
  📦 Order #{order.orderId}
</h5>
<p>
  Status:{" "}

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

</p>

            <p>
              Date:
              {" "}
              {new Date(
                order.orderDate
              ).toLocaleDateString()}
            </p>
<div className="d-flex justify-content-between align-items-center mb-3">
  <h3 className="mb-0">
    Total: ₹{order.totalAmount}
  </h3>

 <button
  className="btn btn-outline-primary"
  onClick={() =>
    handleDownloadInvoice(
      order.orderId
    )
  }
>
  📄 Download Invoice
</button>
</div>
            <p>
  <strong>Delivery Address:</strong>
  <br />
  {order.fullAddress},
  {order.city},
  {order.state},
  {order.pincode}
</p>

            <hr />

     {order.orderItems.map(item => (
  <div
    key={item.productId}
    className="d-flex justify-content-between align-items-center border-bottom py-3"
  >
    <div className="d-flex align-items-center">

      <img
        src={item.imageUrl}
        alt={item.productName}
        style={{
          width: "70px",
          height: "70px",
          objectFit: "contain"
        }}
        className="me-3"
      />

      <div>
        <strong>{item.productName}</strong>
        <br />
        Qty: {item.quantity}
      </div>

    </div>

    <div>
      ₹{item.subTotal}
    </div>

  </div>
))}
          </div>
        ))
      )}
    </div>
  );
}