import { Container, Row, Col } from "react-bootstrap";
import ProductCard from "./ProductCard";
import { useEffect, useState } from "react";
import { getAllProducts } from "../api/productApi";

function FeaturedProducts() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
     const response = await getAllProducts();

setProducts(response.data.items);
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <Container className="my-5">
      <h3 className="fw-bold mb-4">
        Featured Products
      </h3>

      <Row>
      {products.slice(0, 4).map(product => (
          <Col
            lg={3}
            md={6}
            sm={6}
            xs={12}
            key={product.productId}
          >
            <ProductCard product={product} />
          </Col>
        ))}
      </Row>
    </Container>
  );
}

export default FeaturedProducts;