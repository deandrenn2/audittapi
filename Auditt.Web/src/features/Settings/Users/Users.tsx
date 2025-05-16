import { useState } from "react";
import OffCanvas from "../../../shared/components/OffCanvas/Index";
import { LinkSettings } from "../../Dashboard/LinkSenttings";
import { UserCreate } from "./UsersCreate";
import { Direction } from "../../../shared/components/OffCanvas/Models";
import { useUser } from "./useUser";
import { UserUpdate } from "./UserUpdate";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars, faPen, faUser } from "@fortawesome/free-solid-svg-icons";
import { UserResponseModel } from "../../../routes/Login/LoginModel";
export const Users = () => {
    const { users } = useUser();
    const [visible, setVisible] = useState(false);
    const [user, setUser] = useState<UserResponseModel>();
    const [visibleUpdate, setVisibleUpdate] = useState(false);

    const handleClickDetail = (userSelected: UserResponseModel) => {
        if (userSelected) {
            setUser(userSelected);
            setVisibleUpdate(true)
        }
    }

    return (
        <div className="p-6">
            <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                <LinkSettings />
            </div>

            <div>
                <button onClick={() => setVisible(true)} className="bg-[#392F5A] hover:bg-indigo-900 text-white px-4 py-2 rounded-lg font-semibold mb-2 cursor-pointer">
                    Crear Usuario
                </button>
            </div>

            <div className="grid grid-cols-1 gap-4">
                {users?.map((user) => (
                    <div
                        key={user.id}
                        className="flex items-center space-x-6 border p-4 rounded-xl shadow-md w-max">
                        <div>
                            <img
                                src={user.urlProfile}
                                alt="logo"
                                className="min-w-16 rounded-full w-20 h-20" />
                        </div>

                        <div className="flex flex-col">
                            <span className="font-bold text-sm text-gray-900 uppercase">
                                {user.firstName} {user.lastName}
                            </span>
                            <span className="text-sm text-gray-500">{user.email}</span>
                        </div>
                        <div className="font-bold text-sm text-indigo-900 uppercase">
                            <span>{user.roleName}</span>
                        </div>

                        <div className="flex items-center space-x-1">
                            <div className="w-4 h-4 rounded-full bg-lime-500"></div>
                            <span className="text-sm font-semibold text-lime-600">activo</span>
                        </div>

                        <div className="flex items-center space-x-2">
                            <div className="w-8 h-8 flex items-center justify-center rounded-full border border-blue-300 text-blue-900 cursor-pointer">
                                <FontAwesomeIcon icon={faBars} className="text-blue-400" />
                            </div>

                            <div className="w-8 h-8 flex items-center justify-center rounded-full border border-blue-300 text-blue-900 cursor-pointer">
                                <FontAwesomeIcon icon={faUser} className="text-blue-500" />
                            </div>
                            <div className="w-8 h-8 flex items-center justify-center rounded-full bg-green-200 text-green-600 cursor-pointer">
                                <FontAwesomeIcon icon={faPen} className="" onClick={() => handleClickDetail(user)} />
                            </div>
                        </div>
                    </div>
                ))}
            </div>
            <OffCanvas titlePrincipal='Crear Usuario' visible={visible} xClose={() => setVisible(false)} position={Direction.Right}  >
                <UserCreate />
            </OffCanvas>
            {
                user &&
                <OffCanvas titlePrincipal='Actualizar Usuario' visible={visibleUpdate} xClose={() => setVisibleUpdate(false)} position={Direction.Right}>
                    <UserUpdate data={user} />
                </OffCanvas>
            }
        </div>
    );
}