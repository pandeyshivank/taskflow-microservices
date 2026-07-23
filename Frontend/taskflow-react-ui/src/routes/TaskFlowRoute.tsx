import { Route, Routes } from "react-router-dom";
import UserDashBoard from "../pages/UserDashboard";
import Login from "../pages/Login";
import ProtectedRoute from "../components/common/ProtectedRoutes";
import CreateTask from "../pages/CreateTask";
import UpdateTask from "../pages/UpdateTask";
import DeleteTask from "../pages/DeleteTask";
function TaskflowAppRoutes() {
  return (
    <>
      <Routes>
        <Route path="/" element={<Login />}></Route>
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <UserDashBoard />
            </ProtectedRoute>
          }
        ></Route>
        <Route
          path="/createTask"
          element={
            <ProtectedRoute>
              <CreateTask />
            </ProtectedRoute>
          }
        ></Route>
        <Route
          path="/updateTask/:taskId"
          element={
            <ProtectedRoute>
              <UpdateTask />
            </ProtectedRoute>
          }
        ></Route>
        <Route
          path="/deleteTask/:taskId"
          element={
            <ProtectedRoute>
              <DeleteTask />
            </ProtectedRoute>
          }
        ></Route>
      </Routes>
    </>
  );
}
export default TaskflowAppRoutes;
