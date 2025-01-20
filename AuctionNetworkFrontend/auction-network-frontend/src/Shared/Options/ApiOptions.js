export const baseUrl = "https://localhost:7250/api";
export const authorization = (token) => {
    return {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    };
  };