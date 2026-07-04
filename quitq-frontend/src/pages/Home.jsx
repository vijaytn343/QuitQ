import Hero from "../components/Hero";
import CategorySection from "../components/CategorySection";
import FeaturedProducts from "../components/FeaturedProducts";

import OfferBanner from "../components/OfferBanner";


function Home() {
  return (
    <>
      <Hero />
      <CategorySection />
      <FeaturedProducts />
      
      <OfferBanner />
    
    </>
  );
}

export default Home;