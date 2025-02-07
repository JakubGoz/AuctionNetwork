import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import classes from './ReviewModal.module.scss';
import axios from 'axios';
import { baseUrl } from '../../Shared/Options/ApiOptions';

const ReviewModal = ({ listingId, onClose }) => {
  const [rating, setRating] = useState(0); // Stan dla oceny
  const [description, setDescription] = useState(''); // Stan dla opisu
  const [error, setError] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false); // Stan dla wysyłania żądania

  // Funkcja do autoryzacji
  const authorization = (token) => ({
    headers: {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
  });

  const handleSubmit = async () => {
    setError(null); // Resetuj błędy
    if (rating < 1 || rating > 5) {
      setError('Please select a rating between 1 and 5 stars.');
      return;
    }

    setIsSubmitting(true); // Ustaw stan wysyłania
    try {
      const token = localStorage.getItem('token');
      if (!token) throw new Error('You are not authorized.');

      await axios.post(
        `${baseUrl}/listing/${listingId}/review`,
        { listingId, rating, description },
        authorization(token)
        
      );

      setIsSubmitting(false); // Zakończ wysyłanie
      onClose(); // Zamknij modal po sukcesie
      window.location.reload();
      alert('Review successfully added!');
    } catch (err) {
      setIsSubmitting(false); // Zakończ wysyłanie w przypadku błędu
      setError(err.response?.data?.message || 'Failed to submit review. Please try again.');
    }
  };

  return (
    <div className={classes['modal-container']}>
      <div className={classes['modal-content']}>
        <h3>Rate this listing</h3>
        <div className={classes['star-container']}>
          {[1, 2, 3, 4, 5].map((star) => (
            <FontAwesomeIcon
              key={star}
              icon={faStar}
              className={star <= rating ? classes['selected'] : classes['unselected']}
              onClick={() => setRating(star)} // Ustaw ocenę
            />
          ))}
        </div>
        <textarea
          placeholder="Write your review (optional)"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
        {error && <p className={classes['error']}>{error}</p>}
        <div className={classes['modal-actions']}>
          <button onClick={onClose} disabled={isSubmitting}>
            Cancel
          </button>
          <button onClick={handleSubmit} disabled={isSubmitting}>
            {isSubmitting ? 'Submitting...' : 'Submit'}
          </button>
        </div>
      </div>
    </div>
  );
};

export default ReviewModal;
