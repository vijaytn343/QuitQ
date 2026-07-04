import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getMyAddress, updateAddress } from "../api/addressApi";
import { toast } from "react-toastify";
export default function EditAddress() {

  const { id } = useParams();
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    fullAddress: "",
    city: "",
    state: "",
    pincode: "",
    country: ""
  });

  useEffect(() => {
    loadAddress();
  }, []);

  const loadAddress = async () => {
  try {
    const response = await getMyAddress();

    const address = response.data.find(
      a => a.addressId === Number(id)
    );

    if (address) {
      setFormData({
        fullAddress: address.fullAddress,
        city: address.city,
        state: address.state,
        pincode: address.pincode,
        country: address.country
      });
    }
  } catch (error) {
    console.log(error);
  }
};

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await updateAddress(id, formData);

    toast.success("Address Updated Successfully");

      navigate("/my-address");
    } catch (error) {
  console.log(error);

  toast.error(
    error.response?.data?.message || "Failed to update address"
  );
}
  };

  return (
    <div className="container mt-4">
      <h2>Edit Address</h2>

      <form onSubmit={handleSubmit}>
        <div className="card p-4">

          <input
            type="text"
            name="fullAddress"
            className="form-control mb-3"
            value={formData.fullAddress}
            onChange={handleChange}
          />

          <input
            type="text"
            name="city"
            className="form-control mb-3"
            value={formData.city}
            onChange={handleChange}
          />

          <input
            type="text"
            name="state"
            className="form-control mb-3"
            value={formData.state}
            onChange={handleChange}
          />

          <input
            type="text"
            name="pincode"
            className="form-control mb-3"
            value={formData.pincode}
            onChange={handleChange}
          />

          <input
            type="text"
            name="country"
            className="form-control mb-3"
            value={formData.country}
            onChange={handleChange}
          />

          <button
            type="submit"
            className="btn btn-warning"
          >
            Update Address
          </button>

        </div>
      </form>
    </div>
  );
}