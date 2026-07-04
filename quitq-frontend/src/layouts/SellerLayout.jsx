import SellerSidebar from "../components/SellerSidebar";

export default function SellerLayout({ children }) {
  return (
    <div className="d-flex">
      <SellerSidebar />

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