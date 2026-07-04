import { useState } from "react";
import { createAddress } from "../api/addressApi";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
export default function AddAddress() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    fullAddress: "",
    city: "",
    state: "",
    pincode: "",
    country: ""
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await createAddress(formData);

     toast.success("Address Added Successfully");
      navigate("/my-address");
    }
    catch (error) {
      console.log(error);
     toast.error(error.response?.data?.message || "Failed to add address");
    }
  };

  return (
    <div className="container mt-4">
      <h2>Add Address</h2>

      <form onSubmit={handleSubmit}>
        <div className="card p-4">

          <label className="form-label">
             Address
          </label>
          <textarea
            name="fullAddress"
            className="form-control mb-3"
            value={formData.fullAddress}
            onChange={handleChange}
          />

          <label className="form-label">
            City
          </label>
          <input
            type="text"
            name="city"
            className="form-control mb-3"
            value={formData.city}
            onChange={handleChange}
          />

          <label className="form-label">
            State
          </label>
          <input
            type="text"
            name="state"
            className="form-control mb-3"
            value={formData.state}
            onChange={handleChange}
          />

          <label className="form-label">
            Pincode
          </label>
          <input
            type="text"
            name="pincode"
            className="form-control mb-3"
            value={formData.pincode}
            onChange={handleChange}
          />

          <label className="form-label">
            Country
          </label>
          <input
            type="text"
            name="country"
            className="form-control mb-3"
            value={formData.country}
            onChange={handleChange}
          />

          <button
            type="submit"
            className="btn btn-primary"
          >
            Save Address
          </button>

        </div>
      </form>
    </div>
  );
}