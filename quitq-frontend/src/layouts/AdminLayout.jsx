import AdminSidebar from "../components/AdminSidebar";

export default function AdminLayout({ children }) {

  return (

    <div className="d-flex">

      <AdminSidebar />

      <div
        className="flex-grow-1"
        style={{
          marginLeft: "250px",
          minHeight: "100vh",
          background: "#F8FAFC",
          padding: "30px"
        }}
      >

        {children}

      </div>

    </div>

  );

}