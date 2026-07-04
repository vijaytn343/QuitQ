import { useEffect, useState } from "react";
import {
  getCart,
  removeCartItem,
  clearCart
} from "../api/cartApi";
import { useNavigate } from "react-router-dom";
import {
  updateQuantity
}
from "../api/cartApi";
import { toast } from "react-toastify";


export default function Cart() {
  const navigate = useNavigate();
  const [cart, setCart] = useState(null);

  useEffect(() => {
    loadCart();
  }, []);

  const loadCart = async () => {
    try {
      const response = await getCart();
      setCart(response.data);
    } catch (error) {
      console.log(error);
    }
  };

  const handleRemove = async (id) => {
    try {
      await removeCartItem(id);
      toast.success("Item Removed Successfully");

      
      loadCart();
    } catch (error) {
  console.log(error);

  toast.error(
    error.response?.data?.message || "Failed to remove item"
  );
}
  };

  const handleClearCart = async () => {
    try {
      await clearCart();

    toast.success("Cart Cleared Successfully");

      loadCart();
    }catch (error) {
  console.log(error);

  toast.error(
    error.response?.data?.message || "Failed to clear cart"
  );
}
  };
  const changeQuantity =
  async (
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

      loadCart();

    }
   catch (error) {
  console.log(error);

  toast.error(
    error.response?.data?.message || "Stock limit reached"
  );
}
  };

  if (
    !cart ||
    !cart.cartItems ||
    cart.cartItems.length === 0
  ) {
    return (
      <div className="container mt-4">
        <h2>My Cart</h2>
        <p>Cart is empty.</p>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <h2>My Cart</h2>

      {cart.cartItems.map((item) => (
        <div
          key={item.cartItemId}
          className="card p-3 mb-3"
        >
          <div className="row align-items-center">

            <div className="col-md-2">
              <img
                src={item.imageUrl}
                alt={item.productName}
                className="img-fluid"
              />
            </div>

            <div className="col-md-4">
              <h5>{item.productName}</h5>
            </div>

            <div className="col-md-2">
              ₹{item.price}
            </div>

          <div className="col-md-2">

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

            <div className="col-md-2">
              <button
                className="btn btn-danger"
                onClick={() =>
                  handleRemove(item.cartItemId)
                }
              >
                Remove
              </button>
            </div>

          </div>
        </div>
      ))}

      <div className="card p-3">
        <h4>
          Total: ₹{cart.totalAmount}
        </h4>

        <div className="d-flex gap-2 mt-3">

          <button
            className="btn btn-warning"
            onClick={handleClearCart}
          >
            Clear Cart
          </button>
<button
  className="btn btn-success"
  onClick={() => navigate("/checkout")}
>
  Checkout
</button>

        </div>
      </div>
    </div>
  );
}