import { useEffect, useState } from "react";
import { getMyAddress } from "../api/addressApi";
import {
  getCart,
  updateQuantity
} from "../api/cartApi";
import { createOrder } from "../api/orderApi";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

export default function Checkout() {
  const [addresses, setAddresses] = useState([]);
  const [cart, setCart] = useState(null);

  const [selectedAddress, setSelectedAddress] =
    useState("");

  const navigate = useNavigate();

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const addressResponse =
        await getMyAddress();

      const cartResponse =
        await getCart();

      setAddresses(addressResponse.data);
      setCart(cartResponse.data);

      if (addressResponse.data.length > 0) {
       setSelectedAddress(
  parseInt(
    addressResponse.data[0].addressId
  )
);
      }
    } catch (error) {
      console.log(error);
    }
  };

  const handlePlaceOrder = async () => {
    try {
      await createOrder({
        addressId: selectedAddress,
        paymentMethod: "COD"
      });

     toast.success("Order Placed Successfully");

      navigate("/my-orders");
    } catch (error) {
  console.log(error);

  toast.error(
    error.response?.data?.message || "Failed to place order"
  );
}
  };
  const changeQuantity = async (
  cartItemId,
  quantity
) => {

  if (quantity < 1)
    return;

  try {

    await updateQuantity(
      cartItemId,
      quantity
    );

    loadData();

  }
  catch (error) {
  console.log(error);

  toast.error(
    error.response?.data?.message || "Stock limit reached"
  );
}
};

  if (!cart) {
    return <h3>Loading...</h3>;
  }

  return (
    <div className="container mt-4">
      <h2>Checkout</h2>

      <div className="card p-4 mb-3">
        <h4>Select Address</h4>

        <select
          className="form-select"
          value={selectedAddress}
         onChange={(e) =>
  setSelectedAddress(
    parseInt(e.target.value)
  )
}
        >
          {addresses.map((address) => (
            <option
              key={address.addressId}
              value={address.addressId}
            >
              {address.fullAddress},
              {address.city}
            </option>
          ))}
        </select>
      </div>

      <div className="card p-4">
        <h4>Order Summary</h4>

      {cart.cartItems.map((item) => (

<div
  key={item.cartItemId}
  className="border-bottom py-3"
>

<h5>
  {item.productName}
</h5>

<p>
  Price :
  <strong>
    ₹{item.price}
  </strong>
</p>

<div className="d-flex align-items-center mb-2">

<button
  className="btn btn-secondary btn-sm"
  disabled={item.quantity === 1}
  onClick={() =>
    changeQuantity(
      item.cartItemId,
      item.quantity - 1
    )
  }
>
-
</button>

<span className="mx-3">
  {item.quantity}
</span>

<button
  className="btn btn-secondary btn-sm"
  onClick={() =>
    changeQuantity(
      item.cartItemId,
      item.quantity + 1
    )
  }
>
+
</button>

</div>

<p>
Subtotal :
<strong>
₹{item.subTotal}
</strong>
</p>

</div>

))}
        <hr />

        <h4>
          Total: ₹{cart.totalAmount}
        </h4>

        <button
          className="btn btn-success mt-3"
          onClick={handlePlaceOrder}
        >
          Place Order
        </button>
      </div>
    </div>
  );
}