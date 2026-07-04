import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  getCustomerProfile,
  updateCustomerProfile
} from "../api/customerApi";
import { toast } from "react-toastify";

export default function EditUserProfile() {

  const navigate = useNavigate();

  const [profile, setProfile] = useState({
    userId: 0,
    name: "",
    email: "",
    phone: "",
    gender: ""
  });

  useEffect(() => {
    loadProfile();
  }, []);

  const loadProfile = async () => {

    try {

      const response =
        await getCustomerProfile();

      setProfile(response.data);

    } catch (error) {

      console.log(error);

      toast.error("Failed to load profile");

    }

  };

  const handleChange = (e) => {

    setProfile({

      ...profile,

      [e.target.name]: e.target.value

    });

  };

  const handleSubmit = async (e) => {

    e.preventDefault();

    try {

      await updateCustomerProfile(
        profile.userId,
        {
          name: profile.name,
          phone: profile.phone,
          gender: profile.gender
        }
      );

      toast.success("Profile Updated Successfully");

      navigate("/my-profile");

    } catch (error) {

      console.log(error);

      toast.error("Failed to update profile");

    }

  };

  return (

    <div className="container mt-4">

      <h2>Edit Profile</h2>

      <form onSubmit={handleSubmit}>

        <div className="card p-4">

          <label>Name</label>

          <input
            type="text"
            name="name"
            className="form-control mb-3"
            value={profile.name}
            onChange={handleChange}
          />

          <label>Email</label>

          <input
            type="email"
            className="form-control mb-3"
            value={profile.email}
            disabled
          />

          <label>Phone</label>

          <input
            type="text"
            name="phone"
            className="form-control mb-3"
            value={profile.phone || ""}
            onChange={handleChange}
          />

          <label>Gender</label>

          <select
            name="gender"
            className="form-control mb-3"
            value={profile.gender || ""}
            onChange={handleChange}
          >
            <option value="">
              Select Gender
            </option>

            <option value="Male">
              Male
            </option>

            <option value="Female">
              Female
            </option>

            <option value="Other">
              Other
            </option>

          </select>

          <button
            type="submit"
            className="btn btn-warning w-100"
          >
            Update Profile
          </button>

        </div>

      </form>

    </div>

  );

}