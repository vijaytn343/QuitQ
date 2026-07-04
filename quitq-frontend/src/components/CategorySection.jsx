import { Card, Row, Col, Container } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

const categories = [
  { categoryId: 5, name: "Mobiles", icon: "📱" },
  { categoryId: 2, name: "Electronics", icon: "💻" },
  { categoryId: 6, name: "Fashion", icon: "👕" },
  { categoryId: 7, name: "Home", icon: "🏠" },
  { categoryId: 10, name: "Beauty", icon: "💄" },
  { categoryId: 9, name: "Sports", icon: "⚽" },
];

function CategorySection() {
  const navigate = useNavigate();
  return (
    <Container className="my-5">
      <h3 className="mb-4 fw-bold">Shop By Category</h3>

      <Row>
        {categories.map((category, index) => (
          <Col lg={2} md={4} xs={6} key={index} className="mb-4">
           <Card
  className="text-center shadow-sm border-0 h-100"
  style={{ cursor: "pointer" }}
  onClick={() =>
    navigate(
      `/products?categoryId=${category.categoryId}`
    )
  }
>
              <Card.Body>
                <h1>{category.icon}</h1>
                <h6>{category.name}</h6>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
    </Container>
  );
}

export default CategorySection;