import { useState } from "react";
import type { LoginResponse } from "../models/LoginResponse";
import { Link } from "react-router-dom";

interface LoginRequest {
  email: string;
  password: string;
}

function Login() {
  const [loginRequest, setLoginRequest] = useState<LoginRequest>({
    email: "",
    password: "",
  });

  async function handleLogin(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const response = await fetch("https://localhost:7058/api/User/Login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(loginRequest),
    });
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    const loginUserDetail: LoginResponse = await response.json();
    //adding token in local storage
    localStorage.setItem("token", loginUserDetail.token);
    localStorage.setItem("firstName", loginUserDetail.firstName);
    alert(loginUserDetail.firstName);
  }

  return (
    <>
      <Link to="/dashboard"> User Task DashBoard</Link>
      <form onSubmit={handleLogin}>
        <div>
          <h1>Login Form</h1>
        </div>
        <label>Email</label>
        <input
          type="email"
          name="emailid"
          placeholder="Enter user Email Id"
          value={loginRequest.email}
          onChange={(e) =>
            setLoginRequest({ ...loginRequest, email: e.target.value })
          }
        />
        <label>Password</label>
        <input
          type="password"
          name="password"
          placeholder="Enter user Password"
          value={loginRequest.password}
          onChange={(e) =>
            setLoginRequest({ ...loginRequest, password: e.target.value })
          }
        />
        <button type="submit">Login</button>
      </form>
    </>
  );
}
export default Login;
