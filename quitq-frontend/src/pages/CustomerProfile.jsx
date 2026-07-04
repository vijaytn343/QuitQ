import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { getCustomerProfile }
from "../api/customerApi";

import {
  getMyAddress,
  deleteAddress
} from "../api/addressApi";

export default function CustomerProfile() {

  const navigate = useNavigate();

  const [profile, setProfile] =
    useState(null);

  const [addresses, setAddresses] =
    useState([]);

  useEffect(() => {

    loadProfile();

    loadAddresses();

  }, []);

  const loadProfile = async () => {

    try {

      const response =
        await getCustomerProfile();

      setProfile(response.data);

    } catch (error) {

      console.log(error);

    }

  };

  const loadAddresses = async () => {

    try {

      const response =
        await getMyAddress();

      setAddresses(response.data);

    } catch (error) {

      console.log(error);

    }

  };

  if (!profile) {

    return <h3>Loading...</h3>;

  }
  const handleDelete = async (id) => {

  if (!window.confirm("Delete this address?"))
    return;

  try {

    await deleteAddress(id);

    toast.success("Address deleted successfully");

    loadAddresses();

  } catch (error) {

    console.log(error);

    toast.error("Failed to delete address");

  }

};
const handleDeleteAddress = async (id) => {
    if (!window.confirm("Delete this address?"))
        return;

    try {

        await deleteAddress(id);

        toast.success("Address deleted successfully");

        loadAddresses();

    } catch (error) {

        console.log(error);

        toast.error("Failed to delete address");
    }
};

  return (

    <div className="container mt-4">

      <h2>My Profile</h2>

      <div className="card p-4 mb-4">

        <h4>Personal Information</h4>

        <p>

          <strong>Name:</strong>{" "}

          {profile.name}

        </p>

        <p>

          <strong>Email:</strong>{" "}

          {profile.email}

        </p>

        <p>

          <strong>Phone:</strong>{" "}

          {profile.phone || "Not Added"}

        </p>

        <p>

          <strong>Gender:</strong>{" "}

          {profile.gender || "Not Added"}

        </p>

      <div className="d-flex gap-2">

  <button
  className="btn btn-warning btn-sm"
  onClick={() => navigate("/edit-user-profile")}
>
  Edit
</button>
 

 

</div>
      </div>

      <div className="card p-4">

        <div className="d-flex justify-content-between">

          <h4>Saved Addresses</h4>

          <button
            className="btn btn-primary"
            onClick={() =>
              navigate("/add-address")
            }
          >
            + Add Address
          </button>

        </div>

        <hr />

        {addresses.length === 0 ? (

          <p>No Address Added.</p>

        ) : (

          addresses.map(address => (

            <div
              key={address.addressId}
              className="card p-3 mb-3"
            >

              <p>

                <strong>

                  {address.addressType}

                </strong>

              </p>

              <p>

                {address.fullAddress}

              </p>

              <p>

                {address.city},
                {address.state}
              </p>

              <div
                className="d-flex gap-2"
              >

         <button
  className="btn btn-warning btn-sm"
  onClick={() =>
    navigate(`/edit-address/${address.addressId}`)
  }
>
  Edit
</button>
                 <button
    className="btn btn-danger btn-sm ms-2"
    onClick={() => handleDeleteAddress(address.addressId)}
>
    Delete
</button>

              </div>

            </div>

          ))

        )}

      </div>

    </div>

  );

}