import { useEffect, useState } from "react";
import { getMyAddress, deleteAddress } from "../api/addressApi";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

export default function MyAddress() {
  const [addresses, setAddresses] = useState([]);

  const navigate = useNavigate();

  useEffect(() => {
    loadAddress();
  }, []);

  const loadAddress = async () => {
    try {
      const response = await getMyAddress();
      setAddresses(response.data);
    } catch (error) {
      console.log(error);
      toast.error(
    error.response?.data?.message || "Failed to load addresses"
  );
    }
  };

  const handleDelete = async (id) => {
    try {
      await deleteAddress(id);

      toast.success("Address Deleted Successfully");

      loadAddress();
    } catch (error) {
      console.log(error);
      toast.error(
  error.response?.data?.message || "Failed to delete address"
);
    }
  };

  if (addresses.length === 0) {
    return (
      <div className="container mt-4">
        <h2>My Addresses</h2>

        <button
          className="btn btn-primary"
          onClick={() => navigate("/add-address")}
        >
          Add Address
        </button>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <h2>My Addresses</h2>

      <button
        className="btn btn-primary mb-3"
        onClick={() => navigate("/add-address")}
      >
        Add Address
      </button>

      {addresses.map((address) => (
        <div
          key={address.addressId}
          className="card p-4 mb-3"
        >
          <p>
            <strong>Address:</strong>{" "}
            {address.fullAddress}
          </p>

          <p>
            <strong>City:</strong>{" "}
            {address.city}
          </p>

          <p>
            <strong>State:</strong>{" "}
            {address.state}
          </p>

          <p>
            <strong>Pincode:</strong>{" "}
            {address.pincode}
          </p>

          <p>
            <strong>Country:</strong>{" "}
            {address.country}
          </p>

          <div className="d-flex gap-2">
            <button
              className="btn btn-warning"
              onClick={() =>
                navigate(`/edit-address/${address.addressId}`)
              }
            >
              Edit
            </button>

            <button
              className="btn btn-danger"
              onClick={() =>
                handleDelete(address.addressId)
              }
            >
              Delete
            </button>
          </div>
        </div>
      ))}
    </div>
  );
}