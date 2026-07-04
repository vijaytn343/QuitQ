import API from "./axiosConfig";

export const getCart = () => {
  return API.get("/Cart");
};

export const addToCart = (data) => {
  return API.post("/Cart", data);
};

export const updateCartItem = (id, data) => {
  return API.put(`/Cart/${id}`, data);
};

export const removeCartItem = (id) => {
  return API.delete(`/Cart/item/${id}`);
};

export const clearCart = () => {
  return API.delete("/Cart/clear");
};
export const updateQuantity =
  (cartItemId, quantity) =>
    API.put(
      "/Cart/update-quantity",
      {
        cartItemId,
        quantity
      }
    );