import React from "react";
import classes from "./HomePage.module.scss";

function HomePage() {
  return (
    <div className={classes.homeContainer}>
      <header className={classes.header}>
        <h1>Welcome to AuctionNetwork</h1>
        <p>Your trusted marketplace for buying and selling unique items.</p>
      </header>

      <section className={classes.aboutSection}>
        <h2>Why Choose Us?</h2>
        <p>
          At AuctionNetwork, we connect buyers and sellers in a secure and dynamic auction environment.
          Whether you're looking for rare collectibles, high-end electronics, or everyday essentials, our platform
          offers seamless bidding and instant purchase options.
        </p>
      </section>

      <section className={classes.featuresSection}>
        <div className={classes.feature}>
          <h3>Secure Transactions</h3>
          <p>We ensure safe and reliable payments for all users.</p>
        </div>
        <div className={classes.feature}>
          <h3>Real-Time Bidding</h3>
          <p>Engage in exciting auctions and grab the best deals.</p>
        </div>
        <div className={classes.feature}>
          <h3>Buy Now Option</h3>
          <p>Skip the wait and purchase items instantly at a fixed price.</p>
        </div>
      </section>

      <footer className={classes.footer}>
        <p>Start bidding today and discover amazing deals!</p>
      </footer>
    </div>
  );
}

export default HomePage;
